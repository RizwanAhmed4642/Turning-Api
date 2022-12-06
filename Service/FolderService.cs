using AutoMapper;
using Meeting_App.Data.Database.Tables;
using Meeting_App.Models.DTOs;
using Meeting_App.Service.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Meeting_App.Service
{
    public class FolderService
    {
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        public FolderService(IMapper mapper, IWebHostEnvironment env)
        {
            _mapper = mapper;
            _env = env;
        }
        public void Save(FolderDTO model)
        {
            using (var db = new IDDbContext())
            {
                try
                {

                    var dbObject = new Folders
                    {
                        Name = model.Name,
                        Description = model.Description,
                        CreatedAt = DateTime.UtcNow.AddHours(5),
                        CreatedBy = model.UserId,
                        EnableFlag = true,
                        Fk_ParentId = model.ParentFolder?.Id

                    };

                    db.Folders.Add(dbObject);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        public List<FolderDTO> FetchFolders(int? id, string searchQuery)
        {
            var list = new List<FolderDTO>();

            using (var db = new IDDbContext())
            {
                var queryDb = db.Folders.Where(x => x.EnableFlag == true).AsQueryable();

                queryDb = searchQuery?.Length > 1 ? queryDb.Where(o => o.Name.Contains(searchQuery)) : queryDb.Where(x => x.Fk_ParentId == id);

                //if (sort != null)
                //{
                //    if (sort.By == "name")
                //    {
                //        queryDb = sort.Order == "asc"
                //            ? queryDb.OrderBy(x => x.Name)
                //            : queryDb.OrderByDescending(x => x.Name);
                //    }

                //    if (sort.By == "creationDate")
                //    {
                //        queryDb = sort.Order == "asc"
                //            ? queryDb.OrderBy(x => x.CreatedAt)
                //            : queryDb.OrderByDescending(x => x.CreatedAt);
                //    }
                //}

                list = queryDb.Select(
                    o => new FolderDTO
                    {
                        Name = o.Name,
                        UserId = o.CreatedBy,
                        Id = o.PkCode,
                        Description = o.Description,
                        FK_ParenId = o.Fk_ParentId
                    }).ToList();
            }

            //using (var db = new IDUSHAREEntities())
            //{
            //    list.AddRange(db.Folders.Where(x => x.EnableFlag == true && x.Fk_ParentId == id).Select(x => new FolderViewModel { Name = x.Name, UserId = x.CreatedBy, Id = x.PkCode, Description = x.Description }).ToList());
            //}

            return list;
        }

        public List<FolderDTO> FetchFoldersAll()
        {
            var list = new List<FolderDTO>();

            using (var db = new IDDbContext())
            {
                list.AddRange(db.Folders.Where(x => x.EnableFlag == true && x.Fk_ParentId==null).Select(x => new FolderDTO { Name = x.Name, UserId = x.CreatedBy, Id = x.PkCode, Description = x.Description }).ToList());
            }

            return list;
        }


        public async Task<int> AddFile(FileDTO model, string userid)
        {

            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {

                        var newFile = this._mapper.Map<Files>(model);
                       
                        if (model.Fk_Folder != 0 )
                        {
                           
                            foreach (var item in model.FileAttachments)
                            {
                                newFile.EnableFlag = true;
                                newFile.CreatedBy = userid;
                                newFile.CreatedAt = UtilService.GetPkCurrentDateTime();
                                newFile.Description = model.Description;
                                newFile.Name = model.Name;
                                newFile.Path = await UploadFile(item);
                                newFile.Fk_Folder = newFile.Fk_Folder;
                                newFile.Extension = item.ContentType;
                                newFile.FileSize = item.Length.ToString();
                            }
                          
                            db.Files.Add(newFile);

                            db.SaveChanges();

                            trans.Commit();
                            return 1;

                        }
                        else
                        {
                            foreach (var item in model.FileAttachments)
                            {
                                newFile.EnableFlag = true;
                                newFile.CreatedBy = userid;
                                newFile.CreatedAt = UtilService.GetPkCurrentDateTime();
                                newFile.Description = model.Description;
                                newFile.Name = model.Name;
                                newFile.Path = await UploadFile(item);
                                newFile.Fk_Folder = null;
                                newFile.Extension = item.ContentType;
                                newFile.FileSize = item.Length.ToString();
                            }

                            db.Files.Add(newFile);

                            db.SaveChanges();

                            trans.Commit();
                            return 0;

                        }

                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw;
                    }
                }


            }
        }

        public List<FileDTO> FetchFiles(int id, string searchQuery, Guid userid)
        {
            var list = new List<FileDTO>();

            using (var db = new IDDbContext())
            {
                var queryDb = db.Files.Where(x => x.EnableFlag == true).AsQueryable();

                if (id==0)
                {
                    queryDb = searchQuery?.Length > 1 ? queryDb.Where(o => o.Name.Contains(searchQuery)) : queryDb.Where(x => x.Fk_Folder ==null );

                }
                else
                {
                    queryDb = searchQuery?.Length > 1 ? queryDb.Where(o => o.Name.Contains(searchQuery)) : queryDb.Where(x => x.Fk_Folder == id);

                }


                //if (sort != null)
                //{
                //    if (sort.By == "name")
                //    {
                //        queryDb = sort.Order == "asc"
                //            ? queryDb.OrderBy(x => x.Name)
                //            : queryDb.OrderByDescending(x => x.Name);
                //    }

                //    if (sort.By == "creationDate")
                //    {
                //        queryDb = sort.Order == "asc"
                //            ? queryDb.OrderBy(x => x.CreatedAt)
                //            : queryDb.OrderByDescending(x => x.CreatedAt);
                //    }
                //}

                list = queryDb.Select(
                    o => new FileDTO
                    {
                        Name = o.Name,
                        Description = o.Description,
                        FileAttachmentName=o.Name,
                        Path= "/Uploads/" + o.Path,
                        PkCode=o.PkCode


                    }).ToList();
            }

            //using (var db = new IDUSHAREEntities())
            //{
            //    list.AddRange(db.Folders.Where(x => x.EnableFlag == true && x.Fk_ParentId == id).Select(x => new FolderViewModel { Name = x.Name, UserId = x.CreatedBy, Id = x.PkCode, Description = x.Description }).ToList());
            //}

            return list;
        }


        #region HelperMethods
        public async Task<string> UploadFile(IFormFile file)
        {
            try
            {
                string filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                filename = this.EnsureCorrectFilename($"{Guid.NewGuid().ToString("N")}_{ DateTime.UtcNow.AddHours(5).ToString("ddMMyyyyHHmmssffffff")}");
                filename = filename + ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                string path = _env.WebRootPath + "/Uploads/" + filename;


                using (FileStream output = System.IO.File.Create(path))
                    await file.CopyToAsync(output);



                return filename;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            return filename;
        }


        #endregion


        public void Deletefolder(int Id)
        {

            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {

                        if (Id != 0)
                        {

                            var folderDelete = db.Folders.FirstOrDefault(x => x.PkCode == Id);

                            folderDelete.EnableFlag = false;

                            db.SaveChanges();

                            trans.Commit();

                        }

                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }

            }
        }


        public void DeleteFile(int Id)
        {

            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {

                        if (Id != 0)
                        {

                            var fileDelete = db.Files.FirstOrDefault(x => x.PkCode == Id);

                            fileDelete.EnableFlag = false;

                            db.SaveChanges();

                            trans.Commit();

                        }

                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }

            }
        }
        
    }
}

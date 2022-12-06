using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Service.Common
{
    public class PaginationOptions<T> where T: class
    {
            public PaginationOptions()
            {
                Entities = new List<T>();
            }
            public PaginationOptions(int page, int pageSize, int totalRecords, List<T> entities)
            {
                Page = page;
                PageSize = pageSize;
                TotalRecords = totalRecords;
                Entities = entities;
            }
            public int Draw { get; set; }
            public int Page { get; set; }
            public int PageSize { get; set; }
            public int TotalRecords { get; set; }
            public int Pages { get; set; }
            public int NumberSkipped { get; set; }
            public int? NextPage { get; set; }
            public int? PriorPage { get; set; }
            public int FirstPage { get; set; }
            public int LastPage { get; set; }
            public bool OnFirstPage { get; set; }
            public bool OnLastPage { get; set; }
            public bool HasNextPage { get; set; }
            public bool HasPreviousPage { get; set; }
            public List<T> Entities { get; set; }
        }

        public class PaginationViewModel
        {
            public PaginationViewModel()
            {
                Filters = new List<FilterViewModel>();
                Sorts = new List<SortViewModel>();
            }
            public int Draw { get; set; }
            public int Page { get; set; }
            public int PageSize { get; set; }
            public int TotalRecords { get; set; }
            public string Search { get; set; }
            public string SortBy { get; set; }
            public string SortByFieldName { get; set; }
            public int SortIndex { get; set; } = -1;
            public bool ShowRevoked { get; set; }
            public string MeetingAgendaId { get; set; }
            public string RequestFromModule { get; set; }
            public string pkcode { get; set; }
            public string GeoLvl { get; set; }
            public DateTime? From { get; set; }
            public DateTime? To { get; set; }
            public int? LabId { get; set; }
            public List<ColumnViewModel> Columns { get; set; }
            public List<FilterViewModel> Filters { get; set; }
            public List<SortViewModel> Sorts { get; set; }

            public string DivCode { get; set; }
            public string DisCode { get; set; }
            public string TehCode { get; set; }
            public string UCCode { get; set; }
            public string SupervisorCode { get; set; }
            public DateFilter DateFilter { get; set; }
            public string DsgCode { get; set; }
            public int? SessionCode { get; set; }
            public string VisitType { get; set; }
            public bool isLab { get; set; }
            public string UserId { get; set; }
        }
        public class PaginationResult<T> where T : class
        {
            public int Skip { get; set; }
            public int PageSize { get; set; }
            public string Draw { get; set; }
            public int RecordsFiltered { get; set; }

            public int RecordsTotal { get; set; }

            public int? TotalUCCMO { get; set; }
            public int? TotalAIC { get; set; }

            public int TotalRegisteredUCCMO { get; set; }
            public int TotalRegisteredAIC { get; set; }

            public List<T> Data { get; set; }
        }
        public class DateFilter
        {
            public DateTime from { get; set; }
            public DateTime to { get; set; }
        }

        public class ColumnViewModel
        {
            public string DisplayName { get; set; }
            public string ColumnName { get; set; }
        }

        public class FilterViewModel
        {
            public FilterViewModel()
            {
                Values = new List<string>();
            }
            public string Name { get; set; }
            public string Value { get; set; }
            public List<string> Values { get; set; }
            public string ValueTo { get; set; }
            public string Function { get; set; }
            public string Type { get; set; }
            public string Format { get; set; }
            public string DateFrom { get; set; }
            public string DateTo { get; set; }

            public string Operator { get; set; }
            public FilterViewModel Condition1 { get; set; }
            public FilterViewModel Condition2 { get; set; }
            public bool IsCustom { get; set; }
        }

        public class SortViewModel
        {
            public string Name { get; set; }
            public string Sort { get; set; }
            public string Type { get; set; }
        }
    }


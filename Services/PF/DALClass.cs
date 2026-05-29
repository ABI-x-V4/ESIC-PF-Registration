using ClosedXML.Excel;
using DataModels;
using Insfrastructure.DbModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repository.PF;
using System.Data;
using System.Text;

namespace Services.PF
{
    public class DALClass : IPfRegistration
    {
        private readonly EsicPfRegistrationDbContext _context;
        private readonly IConfiguration _configuration;
        public DALClass(EsicPfRegistrationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<List<PfRegistrationDTO>> GetAllPFEmp()
        {
            var data = await _context.PfRegistrations.Select(x => new PfRegistrationDTO
            {
                Id = x.Id,
                AadhaarFullName = x.AadhaarFullName,
                AadhaarDob = x.AadhaarDob,
                AadhaarGender = x.AadhaarGender,
                AadhaarNo = x.AadhaarNo,
                PanFullName = x.PanFullName,
                PanDob = x.PanDob,
                PanGender = x.PanGender,
                PanNo = x.PanNo,
                Nationality = x.Nationality,
                FatherOrHusbandname = x.FatherOrHusbandname,
                FatherOrHusband = x.FatherOrHusband,
                MaritalStatus = x.MaritalStatus,
                MobileNo = x.MobileNo,
                Emailid = x.Emailid,
                Qualification = x.Qualification,
                Doj = x.Doj,
                Aadhaarpath = x.Aadhaarpath,
                PanPath = x.PanPath,
                Uan = x.Uan,
                CreatedDate = x.CreatedDate
            }).ToListAsync();
            return data;
        }
        public async Task<PfRegistrationDTO> GetPFEmpById(int id)
        {
            var data = await GetAllPFEmp();
            return data.FirstOrDefault(e => e.Id == id)!;
        }
        public async Task<PaginatedResult<EmployeeListRowDto>> GetPfEmployeesPagedAsync(int page, int pageSize, string? search, string? gender, string sortBy, string sortDir)
        {

            var baseQuery = _context.PfRegistrations
                            .AsNoTracking()
                            .Select(e => new
                            {
                                e.Id,
                                e.AadhaarFullName,
                                e.AadhaarGender,
                                e.AadhaarDob,
                                e.AadhaarNo,
                                e.PanFullName,
                                e.PanGender,
                                e.PanDob,
                                e.PanNo,
                                e.Nationality,
                                e.FatherOrHusband,
                                e.FatherOrHusbandname,
                                e.MaritalStatus,
                                e.MobileNo,
                                e.Emailid,
                                e.Qualification,
                                e.Doj,
                                e.Uan
                            });

            var total = await baseQuery.CountAsync();

            if (!string.IsNullOrWhiteSpace(gender))
                baseQuery = baseQuery.Where(x => x.AadhaarGender == gender);

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                baseQuery = baseQuery.Where(x =>
                    (x.AadhaarFullName ?? "").ToLower().Contains(search) ||
                    (x.MobileNo ?? "").ToLower().Contains(search) ||
                    (x.Emailid ?? "").ToLower().Contains(search) ||
                    (x.PanNo ?? "").ToLower().Contains(search) ||
                    (x.AadhaarNo ?? "").ToLower().Contains(search) ||
                    (x.AadhaarGender ?? "").ToLower().Contains(search)
                );
            }

            var filtered = await baseQuery.CountAsync();

            bool desc = string.Equals(sortDir, "desc", StringComparison.OrdinalIgnoreCase);

            baseQuery = (sortBy?.ToLower()) switch
            {
                "name" => desc ? baseQuery.OrderByDescending(x => x.AadhaarFullName) : baseQuery.OrderBy(x => x.AadhaarFullName),
                "mobile" => desc ? baseQuery.OrderByDescending(x => x.MobileNo) : baseQuery.OrderBy(x => x.MobileNo),
                "email" => desc ? baseQuery.OrderByDescending(x => x.Emailid) : baseQuery.OrderBy(x => x.Emailid),
                "dob" => desc ? baseQuery.OrderByDescending(x => x.AadhaarDob) : baseQuery.OrderBy(x => x.AadhaarDob),
                _ => desc ? baseQuery.OrderByDescending(x => x.Id) : baseQuery.OrderBy(x => x.Id),
            };

            var items = await baseQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new EmployeeListRowDto
                {
                    EmployeeId = x.Id,
                    Name = x.AadhaarFullName,
                    Mobile = x.MobileNo,
                    Email = x.Emailid,
                    Gender = x.AadhaarGender,
                    Dob = x.AadhaarDob,
                    PanNo = x.PanNo,
                    AadhaarNo = x.AadhaarNo
                })
                .ToListAsync();

            return new PaginatedResult<EmployeeListRowDto>
            {
                Items = items,
                TotalRecords = total,
                FilteredRecords = filtered,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<string> SavePFEmployee(PfRegistrationDTO dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var entity = new PfRegistration
                {
                    AadhaarFullName = dto.AadhaarFullName,
                    AadhaarDob = dto.AadhaarDob!.Value,
                    AadhaarGender = dto.AadhaarGender,
                    AadhaarNo = dto.AadhaarNo,
                    PanFullName = dto.PanFullName,
                    PanDob = dto.PanDob!.Value,
                    PanGender = dto.PanGender,
                    PanNo = dto.PanNo,
                    Nationality = dto.Nationality,
                    FatherOrHusbandname = dto.FatherOrHusbandname,
                    FatherOrHusband = dto.FatherOrHusband,
                    MaritalStatus = dto.MaritalStatus,
                    MobileNo = dto.MobileNo,
                    Emailid = dto.Emailid,
                    Qualification = dto.Qualification,
                    Doj = dto.Doj!.Value,
                    Aadhaarpath = dto.Aadhaarpath!,
                    PanPath = dto.PanPath!,
                    Uan = dto.Uan,
                    CreatedDate = DateTime.Now,
                    EmployeeId= dto.EmployeeId
                };
                _context.PfRegistrations.Add(entity);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return $"Error: {ex.Message}";
            }
        }
        public async Task<byte[]> ExportPFEmployeeFullReportAsync(EmployeeListRowDto search)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            var Query = BuildEmployeeQuery(search, out List<SqlParameter> employeeParams);

            DataTable employeeTable = await ExecuteQueryAsync(connectionString, Query, employeeParams);

            using var workbook = new XLWorkbook();

            AddWorksheet(workbook, employeeTable, "Employee PF Details");

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        private static void AddWorksheet(XLWorkbook workbook, DataTable dt, string sheetName)
        {
            var ws = workbook.Worksheets.Add(sheetName);

            ws.Cell(1, 1).Value = "AS PER AADHAAR";
            ws.Range(1, 1, 1, 4).Merge();

            ws.Cell(1, 5).Value = "AS PER PAN";
            ws.Range(1, 5, 1, 8).Merge();

            string[] headers =
            {
                "NAME",
                "DOB",
                "GENDER",
                "AADHAAR",

                "NAME",
                "DOB",
                "GENDER",
                "PAN",

                "NATIONALITY",
                "FATHER OR HUSBAND",
                "FATHER OR HUSBAND NAME",
                "MARITAL STATUS",
                "MOBILE",
                "EMAIL ID",
                "QUALIFICATION",
                "DOJ"
            };

            for (int i = 0; i < headers.Length; i++)
            {
                ws.Cell(2, i + 1).Value = headers[i];
            }

            int row = 3;

            foreach (DataRow dr in dt.Rows)
            {
                for (int col = 0; col < dt.Columns.Count; col++)
                {
                    if (dr[col] != DBNull.Value && dr[col] is DateTime dtValue)
                    {
                        ws.Cell(row, col + 1).Value = dtValue.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        ws.Cell(row, col + 1).Value = dr[col]?.ToString();
                    }
                }

                row++;
            }

            var topHeader = ws.Range(1, 1, 1, headers.Length);
            topHeader.Style.Font.Bold = true;
            topHeader.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            topHeader.Style.Fill.BackgroundColor = XLColor.LightGray;

            var headerRange = ws.Range(2, 1, 2, headers.Length);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.RichBlack;
            headerRange.Style.Font.FontColor = XLColor.White;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Columns().AdjustToContents();
            ws.SheetView.FreezeRows(2);

            ws.RangeUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.RangeUsed().Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        }

        private async Task<DataTable> ExecuteQueryAsync(string connectionString, string query, List<SqlParameter> parameters)
        {
            var dt = new DataTable();

            await using var con = new SqlConnection(connectionString);
            await using var cmd = new SqlCommand(query, con);

            if (parameters != null && parameters.Count > 0)
                cmd.Parameters.AddRange(parameters.ToArray());

            await con.OpenAsync();

            await using var reader = await cmd.ExecuteReaderAsync();
            dt.Load(reader);

            return dt;
        }
        private string BuildEmployeeQuery(EmployeeListRowDto search, out List<SqlParameter> parameters)
        {
            parameters = new List<SqlParameter>();

            var sql = new StringBuilder(@"
SELECT
    Aadhaar_FullName      AS AadhaarName,
    Aadhaar_Dob           AS AadhaarDob,
    Aadhaar_Gender        AS AadhaarGender,
    AadhaarNo             AS AadhaarNo,

    Pan_FullName          AS PanName,
    Pan_Dob               AS PanDob,
    Pan_Gender            AS PanGender,
    PanNo                 AS PanNo,

    Nationality           AS Nationality,
    FatherOrHusband       AS FatherOrHusband,
    FatherOrHusbandname   AS FatherOrHusbandName,
    MaritalStatus         AS MaritalStatus,
    MobileNo              AS Mobile,
    Emailid               AS EmailId,
    Qualification         AS Qualification,
    DOJ                   AS DOJ

FROM PF_Registration e
WHERE 1=1
");
            AppendCommonFilters(sql, parameters, search, includeMobileFilter: true);

            sql.Append(" ORDER BY Id DESC");

            return sql.ToString();
        }
        private void AppendCommonFilters(StringBuilder sql, List<SqlParameter> parameters, EmployeeListRowDto search, bool includeMobileFilter)
        {
            if (search.EmployeeId > 0)
            {
                sql.Append($" AND Id = @EmployeeId");
                parameters.Add(new SqlParameter("@EmployeeId", search.EmployeeId));
            }

            if (!string.IsNullOrWhiteSpace(search.Name))
            {
                sql.Append($" AND Aadhaar_FullName LIKE @Name");
                parameters.Add(new SqlParameter("@Name", $"%{search.Name.Trim()}%"));
            }

            if (!string.IsNullOrWhiteSpace(search.AadhaarNo))
            {
                sql.Append($" AND AadhaarNo LIKE @AadhaarNo");
                parameters.Add(new SqlParameter("@AadhaarNo", $"%{search.AadhaarNo.Trim()}%"));
            }

            if (!string.IsNullOrWhiteSpace(search.PanNo))
            {
                sql.Append($" AND PanNo LIKE @PanNo");
                parameters.Add(new SqlParameter("@PanNo", $"%{search.PanNo.Trim()}%"));
            }

            if (includeMobileFilter && !string.IsNullOrWhiteSpace(search.Mobile))
            {
                sql.Append(" AND MobileNo LIKE @MobileNo");
                parameters.Add(new SqlParameter("@MobileNo", $"%{search.Mobile.Trim()}%"));
            }
        }

    }
}

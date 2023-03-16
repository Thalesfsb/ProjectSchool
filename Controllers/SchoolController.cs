using Dapper;
using Microsoft.AspNetCore.Mvc;
using ProjectSchool.Entities;
using ProjectSchool.Models;
using System.Data.SqlClient;

namespace ProjectSchool.Controllers
{
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private readonly string? _connectionString;
        public SchoolController(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("SchoolDB");

        [HttpGet("api/school")]
        public async Task<IActionResult> Get()
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    const string sql = "SELECT * FROM Student WHERE IsActive = 1";
                    var student = await sqlConnection.QueryAsync<Student>(sql);
                    return Ok(student);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Could not get students {ex.Message} ");
            }
        }

        [HttpGet("api/school/{id}")]
        public async Task<IActionResult> GetId(int id)
        {
            var parameters = new { id };

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    const string sql = "SELECT * FROM Student WHERE Id = @id";
                    var student = await sqlConnection.QuerySingleOrDefaultAsync<Student>(sql, parameters);

                    if (student is null)
                        return NotFound();

                    return Ok(student);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Could not get by id student {ex.Message}");
            }
        }

        [HttpPost("api/school")]
        public async Task<IActionResult> Post(StudentInputModel model)
        {
            var student = new Student(model.FullName, model.SchollClass, model.BirthDate);
            var parameters = new
            {
                student.FullName,
                student.SchollClass,
                student.BirthDate,
                student.IsActive
            };

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    const string sql = "INSERT INTO Student OUTPUT INSERTED.Id VALUES (@FullName, @SchollClass, @BirthDate, @IsActive)";
                    var id = await sqlConnection.ExecuteScalarAsync<int>(sql, parameters);
                    return Ok(id);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Could not post student{ex.Message}");
            }
        }

        [HttpPut("api/school/{id}")]
        public async Task<IActionResult> Put(int id, StudentInputModel model)
        {
            var paramenters = new { id };

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    const string sql = "SELECT * FROM Student WHERE Id = @id";
                    var student = await sqlConnection.QuerySingleOrDefaultAsync<Student>(sql, paramenters);

                    if (student is null)
                        return NotFound("Studante não encontrado para atualização");

                    var studentUpd = new Student(model.FullName, model.SchollClass, model.BirthDate);

                    var paramtersUpd = new
                    {
                        id,
                        studentUpd.FullName,
                        studentUpd.SchollClass,
                        studentUpd.BirthDate,
                        studentUpd.IsActive
                    };

                    const string sqlUpd = "UPDATE Student SET FullName = @FullName, SchoolClass = @SchoolClass, BirthDate = @BirthDate, IsActive = @IsActive WHERE Id = @id";
                    await sqlConnection.ExecuteAsync(sqlUpd, paramtersUpd);
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Could note update {ex.Message}");
            }
        }

        [HttpDelete("api/school/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var paramenters = new { id };

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    const string sql = "UPDATE Student SET IsActive = 0 WHERE ID = @id";
                    await sqlConnection.ExecuteAsync(sql, paramenters);
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Could not delete {ex.Message}");
            }
        }

        [HttpPut("api/school/enableUser/{id}")]
        public async Task<IActionResult> EnableUser(int id)
        {
            var paramenters = new { id };

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    const string sql = "UPDATE Student SET IsActive = 1 WHERE ID = @id";
                    await sqlConnection.ExecuteAsync(sql, paramenters);
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Could not enable user {ex.Message}");
            }
        }
    }
}

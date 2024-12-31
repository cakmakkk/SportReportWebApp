using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using VeriTabaniOdev.CRUD;
using VeriTabaniOdev.Models;

namespace VeriTabaniOdev.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FileDibiDbSonContext _context;
        private readonly string _connectionString = "Host=localhost;Database=fileDibiDbSon4;Username=postgres;Password=cakmak99";
        public HomeController(ILogger<HomeController> logger, FileDibiDbSonContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index(string league = null, int? sportID = null)
        {
            var sports = await _context.Sports
                .Select(s => new { s.Sportid, s.Sportname })
                .ToListAsync();

            if (!sports.Any())
            {
                _logger.LogError("No sports found in the database.");
                return View("Error", "No sports available.");
            }

            var selectedSport = sportID ?? sports.First().Sportid;

            var leagues = await _context.Leagues
                .FromSqlRaw("SELECT DISTINCT Leaguename FROM Leagues WHERE Sportid = {0}", selectedSport)
                .Select(l => l.Leaguename)
                .ToListAsync();

            if (!leagues.Any())
            {
                _logger.LogWarning("No leagues found for SportID: {SelectedSport}", selectedSport);
                return View("Error", "No leagues available for the selected sport.");
            }

            var selectedLeague = league ?? leagues.First();

            var pointsTable = await _context.Pointstables
                .FromSqlRaw("SELECT * FROM Pointstable WHERE Leaguename = {0} ORDER BY Points DESC", selectedLeague)
                .ToListAsync();

            if (!pointsTable.Any())
            {
                _logger.LogWarning("Points table is empty for league: {SelectedLeague}", selectedLeague);
            }

            // Pass sports, leagues, and selected sport/league to the view
            ViewBag.Sports = sports;
            ViewBag.Leagues = leagues;
            ViewBag.SelectedSport = selectedSport;
            ViewBag.SelectedLeague = selectedLeague;

            return View(pointsTable);
        }
        [HttpPost]
        public IActionResult CreateFixtureView(Fixture report, int? Leagueid = null)
        {
            if (Leagueid == null || Leagueid <= 0)
            {
                return BadRequest("Geçerli bir Leagueid saðlamalýsýnýz.");
            }

            if (report.Matchdate > new DateOnly(2024, 12, 31))
            {
                report.Homescore = null;
                report.Awayscore = null;
            }

            using (var con = new NpgsqlConnection(_connectionString))
            {
                string query = "INSERT INTO Fixtures (Leagueid, Hometeamid, Awayteamid, Matchdate, Homescore, Awayscore) " +
                               "VALUES (@Leagueid, @HomeTeamId, @AwayTeamId, @mDate, @hScore, @aScore);";

                using (var command = new NpgsqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@Leagueid", Leagueid);
                    command.Parameters.AddWithValue("@HomeTeamId", (object?)report.Hometeamid ?? DBNull.Value);
                    command.Parameters.AddWithValue("@AwayTeamId", (object?)report.Awayteamid ?? DBNull.Value);
                    command.Parameters.AddWithValue("@mDate", report.Matchdate);
                    command.Parameters.AddWithValue("@hScore", (object?)report.Homescore ?? DBNull.Value);
                    command.Parameters.AddWithValue("@aScore", (object?)report.Awayscore ?? DBNull.Value);

                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }

            return RedirectToAction("FixtureView");
        }


        [HttpGet]
        public IActionResult TeamViewByLeague(int Leagueid)
        {
            if (Leagueid <= 0)
            {
                return Content("<p>Geçersiz League ID.</p>", "text/html");
            }

            var teams = _context.Teams
                .Where(t => t.Leagueid == Leagueid)
                .OrderBy(t => t.Teamid)
                .ToList();

            if (!teams.Any())
            {
                return Content("<p>Bu lige ait takým bulunamadý.</p>", "text/html");
            }

            var teamHtml = "<table class='table'><thead><tr><th>Team ID</th><th>Team Name</th><th>League ID</th></tr></thead><tbody>";
            foreach (var team in teams)
            {
                teamHtml += $"<tr><td>{team.Teamid}</td><td>{team.Teamname}</td><td>{team.Leagueid}</td></tr>";
            }
            teamHtml += "</tbody></table>";

            return Content(teamHtml, "text/html");
        }



        public async Task<IActionResult> UIndex(string league = null, int? sportID = null)
        {
            var sports = await _context.Sports
                .Select(s => new { s.Sportid, s.Sportname })
                .ToListAsync();

            if (!sports.Any())
            {
                _logger.LogError("No sports found in the database.");
                return View("Error", "No sports available.");
            }

            var selectedSport = sportID ?? sports.First().Sportid;

            var leagues = await _context.Leagues
                .FromSqlRaw("SELECT DISTINCT Leaguename FROM Leagues WHERE Sportid = {0}", selectedSport)
                .Select(l => l.Leaguename)
                .ToListAsync();

            if (!leagues.Any())
            {
                _logger.LogWarning("No leagues found for SportID: {SelectedSport}", selectedSport);
                return View("Error", "No leagues available for the selected sport.");
            }

            var selectedLeague = league ?? leagues.First();

            var pointsTable = await _context.Pointstables
                .FromSqlRaw("SELECT * FROM Pointstable WHERE Leaguename = {0} ORDER BY Points DESC", selectedLeague)
                .ToListAsync();

            if (!pointsTable.Any())
            {
                _logger.LogWarning("Points table is empty for league: {SelectedLeague}", selectedLeague);
            }

            ViewBag.Sports = sports;
            ViewBag.Leagues = leagues;
            ViewBag.SelectedSport = selectedSport;
            ViewBag.SelectedLeague = selectedLeague;

            return View(pointsTable);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return PartialView();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user == null)
            {
                ViewBag.Message = "Geçersiz kullanýcý adý veya þifre!";
                return PartialView();
            }

            if (user.Role == "admin")
            {
                return RedirectToAction("Index","Home");
            }
            else
            {
                return RedirectToAction("UIndex","Home");
            }
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return PartialView();
        }

        [HttpPost]
        public IActionResult SignUp(string username, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                ViewBag.Message = "Þifreler eþleþmiyor!";
                return PartialView();
            }

            try
            {
                using (var connection = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString))
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand("CALL create_user(@username, @password, @role)", connection))
                    {
                        command.Parameters.AddWithValue("username", username);
                        command.Parameters.AddWithValue("password", password);
                        command.Parameters.AddWithValue("role", "user");

                        command.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("Login", "Home");
            }
            catch (PostgresException ex) when (ex.SqlState == "P0001")
            {
                ViewBag.Message = ex.Message;
                return PartialView();
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Bir hata oluþtu: " + ex.Message;
                return PartialView();
            }
        }
        public IActionResult Profile()
        {
            // Fetch all users from the database
            var users = _context.Users.ToList();

            return View(users); // Pass the list of users to the view
        }
        [HttpPost]
        public IActionResult SearchUser(string username)
        {
            var user = _context.Users
                               .Where(u => u.Username.Contains(username)) // LIKE query equivalent
                               .Select(u => new { u.Userid, u.Username })
                               .FirstOrDefault();

            if (user != null)
            {
                return Json(new { userid = user.Userid, username = user.Username });
            }
            else
            {
                return Json(null); // If no user is found
            }
        }
        [HttpPost]
        public IActionResult UpdateRole(int id, string newRole)
        {
            var user = _context.Users.FirstOrDefault(u => u.Userid == id);
            if (user != null)
            {
                user.Role = newRole;
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
            // Find the user by id
            var user = _context.Users.FirstOrDefault(u => u.Userid == id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Delete the user
            _context.Users.Remove(user);
            _context.SaveChanges();

            return RedirectToAction("Profile"); // Redirect back to the Profile list
        }

        public IActionResult TeamView(string teamname)
        {
            var team = _context.Teams.FirstOrDefault(t => t.Teamname == teamname);
            if (team == null)
            {
                // Handle the case where the team is not found
                return NotFound();
            }
            return View(team);
        }
        public IActionResult UTeamView(string teamname)
        {
            var team = _context.Teams.FirstOrDefault(t => t.Teamname == teamname);
            if (team == null)
            {
                // Handle the case where the team is not found
                return NotFound();
            }
            return View(team);
        }
        public IActionResult EditFixture(int matchId)
        {
            var team = _context.Fixtures.FirstOrDefault(t => t.Matchid == matchId);
            if (team == null)
            {
                // Handle the case where the team is not found
                return NotFound();
            }
            return View(team);
        }

        [HttpPost]
        public IActionResult EditFixtureUpdate(Fixture fixture)
        {
            
            // Veritabaný iþlemleri - Fixture'ý güncelleme iþlemi
            using (var connection = new Npgsql.NpgsqlConnection(_connectionString))
            {
                connection.Open();

                var command = new Npgsql.NpgsqlCommand("CALL update_fixture(@MatchId, @MatchDate, @HomeTeamId, @AwayTeamId, @HomeScore, @AwayScore)", connection);

                command.Parameters.AddWithValue("MatchId", fixture.Matchid);

                command.Parameters.AddWithValue("MatchDate", fixture.Matchdate.ToDateTime(TimeOnly.MinValue)); // DateOnly -> DateTime dönüþümü


                command.Parameters.AddWithValue("HomeTeamId", fixture.Hometeamid);
                command.Parameters.AddWithValue("AwayTeamId", fixture.Awayteamid);

                command.Parameters.AddWithValue("HomeScore", fixture.Homescore ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("AwayScore", fixture.Awayscore ?? (object)DBNull.Value);

                command.ExecuteNonQuery();
                connection.Close();
            }
            return RedirectToAction("FixtureView");
        }


        public IActionResult UpdateFixtureView()
        {
            return View();
        }

        public IActionResult FixtureView()
        {
            var fixtures = _context.Fixtures
                                   .Include(f => f.Hometeam)
                                   .Include(f => f.Awayteam)
                                   .Include(f => f.League)
                                   .OrderBy(f => f.Matchdate) 
                                   .ToList();
            return View(fixtures); 
        }

        public IActionResult UFixtureView()
        {
            var fixtures = _context.Fixtures
                                   .Include(f => f.Hometeam)
                                   .Include(f => f.Awayteam)
                                   .Include(f => f.League)
                                   .OrderBy(f => f.Matchdate) 
                                   .ToList();

            return View(fixtures); 
        }

        [HttpPost]
        public IActionResult DeleteFixture(int matchId)
        {
            using (var connection = new Npgsql.NpgsqlConnection(_connectionString))
            {
                connection.Open();

                var command = new Npgsql.NpgsqlCommand("CALL delete_fixture(@MatchId)", connection);
                    command.Parameters.AddWithValue("Matchid", matchId); 

                command.ExecuteNonQuery();  

                connection.Close();
            }

            return RedirectToAction("FixtureView");
        }
        


        public async Task<IActionResult> UpdateMatch(string teamName)
        {
            if (string.IsNullOrEmpty(teamName))
            {
                _logger.LogError("Team name cannot be null or empty.");
                return View("Error", "Invalid team name.");
            }

            var pointsTable = await _context.Pointstables
                .FromSqlRaw("SELECT * FROM Pointstable WHERE Teamname = {0} ORDER BY Points DESC", teamName)
                .FirstOrDefaultAsync();

            if (pointsTable == null)
            {
                _logger.LogWarning("No team found with name: {TeamName}", teamName);
                return View("Error", "Team not found.");
            }

            return View(pointsTable);
        }
        [HttpPost]
        public IActionResult UpdateTeam(Team updatedTeam)
        {
            if (updatedTeam == null)
            {
                return BadRequest("Team data is null.");
            }

            using (var con = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    string query = "CALL update_team(@Teamid, @Teamname, @Leagueid, @City, @Points, @Goalsscored, @Goalsconceded, @Wins, @Draws, @Losses, @Leagueposition)";

                    using (var command = new NpgsqlCommand(query, con))
                    {
                        command.Parameters.AddWithValue("@Teamid", updatedTeam.Teamid);
                        command.Parameters.AddWithValue("@Teamname", updatedTeam.Teamname ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Leagueid", (object)updatedTeam.Leagueid ?? DBNull.Value);
                        command.Parameters.AddWithValue("@City", updatedTeam.City ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Points", (object)updatedTeam.Points ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Goalsscored", (object)updatedTeam.Goalsscored ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Goalsconceded", (object)updatedTeam.Goalsconceded ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Wins", (object)updatedTeam.Wins ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Draws", (object)updatedTeam.Draws ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Losses", (object)updatedTeam.Losses ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Leagueposition", (object)updatedTeam.Leagueposition ?? DBNull.Value);

                        con.Open();
                        command.ExecuteNonQuery(); 
                        con.Close();

                        return RedirectToAction("Index", "Home");
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
        }


        [HttpPost]
        public async Task<IActionResult> GetLeaguesBySport(int sportID)
        {
            if (sportID <= 0)
            {
                _logger.LogError("Invalid SportID: {SportID}", sportID);
                return Json(new { error = "Invalid sport ID." });
            }

            var leagues = await _context.Leagues
                .FromSqlRaw("SELECT Leagueid, Leaguename FROM Leagues WHERE Sportid = {0}", sportID)
                .Select(l => new
                {
                    LeagueID = l.Leagueid,
                    LeagueName = l.Leaguename
                })
                .ToListAsync();

            return Json(leagues);
        }

        public async Task<IActionResult> PlayerView(string teamName)
        {
            // Belirli bir takým adýyla filtrelenmiþ oyuncular
            var players = await _context.Players
                .Include(p => p.Team)  // Team bilgilerini yükler
                .Include(p => p.Position) // Eðer Position bilgisi de varsa
                .Where(p => p.Team.Teamname == teamName) // Takým adýna göre filtrele
                .ToListAsync();

            // Oyuncu listesini View'a gönder
            return View(players);
        }


        public async Task<IActionResult> UPlayerView(string teamName)
        {
            // SQL sorgusunu teamName'e göre filtreliyoruz
            var players = await _context.Players
                .FromSqlRaw(@"
            SELECT p.* 
            FROM Players p
            INNER JOIN Teams t ON p.TeamId = t.TeamId
            WHERE t.TeamName = {0}", teamName)  // teamName parametresi ile filtreleme
                .Include(p => p.Team)  // Ýliþkili Team verisini yükler
                .Include(p => p.Position)  // Ýliþkili Position verisini yükler
                .ToListAsync();

            return View(players);
        }

        public IActionResult Detail()
        {
            var players = _context.Players.ToList();
            return View(players);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult UPrivacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

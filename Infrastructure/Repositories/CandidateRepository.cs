using System.Net.Mail;
using Core;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{

    public class CandidateRepository : ICandidateRepository
    {
        ISystematicContext _context;
        public CandidateRepository(ISystematicContext context)
        {
            _context = context;
        }

        //Creates an candidate
        public async Task<(Status, int id)> Create(CreateCandidateDTO candidateDTO) {

            DateTime currentDate = DateTime.Today;
            var dupe = _context.Candidates.Where(c => c.Email == candidateDTO.Email).FirstOrDefault();
            if (dupe != default(Candidate)) return (Status.Conflict, dupe.Id);
                if(!IsValid(candidateDTO.Email)) return (Status.BadRequest, 0);
                //Is this function necessary? Ask Iulia about multiple email entries in the db
                var entity = new Candidate
                {
                    Name = candidateDTO.Name!,
                    Email = candidateDTO.Email!,
                    CurrentDegree = candidateDTO.CurrentDegree,     
                    StudyProgram = candidateDTO.StudyProgram,
                    University = candidateDTO.University,
                    GraduationDate = DateTime.Parse(candidateDTO.GraduationDate),
                    IsUpvoted = candidateDTO.IsUpvoted,
                    Created = currentDate
                };

                _context.Candidates.Add(entity);

                await _context.SaveChangesAsync();

                return (Status.Created, entity.Id);
        }

    public bool IsValid(string emailaddress)
    {
    try
    {
        MailAddress m = new MailAddress(emailaddress);

        return true;
    }
    catch (FormatException)
    {
        return false;
    }
}

        //Return a name given the candidate id 
        public async Task<(Status, string?)> ReadNameFromId(int id) {
            string? name = await _context.Candidates.Where(c => c.Id == id).Select(c => c.Name).FirstOrDefaultAsync();
            return (name == null ? Status.NotFound : Status.Found, name);
        }

        //Return an candidate given the candidate id
        public async Task<(Status, CandidateDTO)> Read(int id)
        {
            var c = await _context.Candidates.Include(c => c.Answer).Where(c => c.Id == id).Select(c => new CandidateDTO(){
                Name = c.Name!,
                Id = c.Id,
                Email = c.Email!,
                CurrentDegree = c.CurrentDegree!, 
                StudyProgram = c.StudyProgram!,
                University = c.University!,
                GraduationDate = c.GraduationDate.ToString("yyyy-MM"),
                IsUpvoted = c.IsUpvoted,
                Created = c.Created,
                PercentageOfCorrectAnswers = c.PercentageOfCorrectAnswers
            }).FirstOrDefaultAsync();

            if (c == default(CandidateDTO)) return (Status.NotFound, c);
            else return (Status.Found, c);
        }

        //Delete candidates from the system who have been there for 2 years and over
        public async Task DeleteOldCandidates(){
            var c = await _context.Candidates.Where(c => c.Created.AddYears(2) < DateTime.Today).ToListAsync();

            foreach (var candi in c){
                _context.Candidates.Remove(candi);
            }

            await _context.SaveChangesAsync();
        }

        //Return a list of all canidates 
         public async Task<IReadOnlyCollection<CandidateDTO>> ReadAll() =>
            await _context.Candidates.Select(c => new CandidateDTO(){
                Name = c.Name!,
                Id = c.Id,
                Email = c.Email!,
                CurrentDegree = c.CurrentDegree!, 
                StudyProgram = c.StudyProgram!,
                University = c.University!,
                GraduationDate = c.GraduationDate.ToString("yyyy-MM"),
                IsUpvoted = c.IsUpvoted,
                Created = c.Created,
                PercentageOfCorrectAnswers = c.PercentageOfCorrectAnswers!
            }).ToListAsync();
        
        //Updates an candidate name, email, university and study program values
        public async Task<Status> Update(int id, CandidateDTO candidateDTO)
        {
            var c = await _context.Candidates.Where(c => c.Id == id).FirstOrDefaultAsync();

            if (c == default(Candidate)) return Status.NotFound;

            c.Name = candidateDTO.Name;
            c.Email = candidateDTO.Email!;
            c.University = candidateDTO.University;
            c.CurrentDegree = candidateDTO.CurrentDegree;
            c.StudyProgram = candidateDTO.StudyProgram;
            c.GraduationDate = DateTime.Parse(candidateDTO.GraduationDate);
            c.IsUpvoted = candidateDTO.IsUpvoted;
            c.Created = candidateDTO.Created;

            await _context.SaveChangesAsync();

            return Status.Updated;
        }
        
        //Upvotes a candidate given the candidate id
        public async Task<Status> UpdateUpVote(int id)
        {
            var c = await _context.Candidates.Where(c => c.Id == id).FirstOrDefaultAsync();

            c.IsUpvoted = true;

            await _context.SaveChangesAsync();

            return Status.Updated;
        }

        //Deletes a candidates given the candidate id
        public async Task<Status> Delete(int id){

            var c = await _context.Candidates.Include(c => c.Answer).Where(c => c.Id == id).FirstOrDefaultAsync();
            
            if (c == default(Candidate)) return Status.NotFound;

            _context.Candidates.Remove(c);

            await _context.SaveChangesAsync();
            return Status.Deleted;
        }

        //Adds quiz answers to the candidate given the candidate id and the answers 
        public async Task<Status> AddAnswer(int candidateId, AnswersDTO answer) {
            var c = await _context.Candidates.Include(c => c.Answer).Include(c => c.EventsParticipatedIn).Where(c => c.Id == candidateId).FirstOrDefaultAsync();

            if (c == default(Candidate)) return Status.NotFound;

            var quiz = await _context.Quizes.Include(q => q.Questions).Where(q => q.Id == answer.QuizId).FirstOrDefaultAsync();
            
            if (quiz == default(Quiz)) return Status.NotFound;

            var ans = new Answer()
            {
                Quiz = quiz,
                Answers = answer.Answers
            };

            c.Answer = ans; //Add the answer to the candidate

            var numOfCorrectAnswers = 0.0;
            
            for (int i = 0; i < ans.Answers?.Length; i++)
            {
                if (ans.Answers[i] == quiz.Questions?.ElementAt(i).Answer)
                {
                    numOfCorrectAnswers++;
                }
            }
            
            c.PercentageOfCorrectAnswers = (numOfCorrectAnswers / quiz.Questions!.Count()) * 100;
            
            var e = await _context.Events.Where(e => e.Id == answer.EventId).FirstOrDefaultAsync();
            if (e != default(Event)) c.EventsParticipatedIn!.Add(e); //Add the candidate to the event

            await _context.SaveChangesAsync();
            return Status.Updated;
        }

        //Do not give me an array with the "Other" field!
        public async Task<int[]> GraphData(IEnumerable<string> universities) {
            var unis = universities.ToArray();
            var generalUniversities = universities.ToHashSet();
            var dict = new Dictionary<string, int>();

            await _context.Candidates.ForEachAsync(candidate => {
                var uni = candidate.University!;
                if (!generalUniversities.Contains(uni)) {
                    if (dict.ContainsKey("Other")) {
                        dict["Other"]++;
                    } else {
                        dict["Other"] = 1;
                    }
                }
                else {
                    if (dict.ContainsKey(uni)) {
                        dict[uni]++;
                    } else {
                        dict[uni] = 1;
                    }
                }
            });

            var data = new int[unis.Length + 1];
            for (int i = 0; i < unis.Length; i++)
            {
                var uni = unis[i];
                data[i] = dict.ContainsKey(uni) ? dict[uni] : 0;
            }
            data[unis.Length] = dict.ContainsKey("Other") ? dict["Other"] : 0;
            return data;
        }

        private HashSet<string> ValidUniversities = new HashSet<string> {
            "Aalborg University",
            "Aarhus University",
            "Copenhagen Business School",
            "IT-University of Copenhagen",
            "Roskilde University",
            "Technical University of Denmark",
            "University of Copenhagen",
            "University of Southern Denmark"};

        public async Task<UniversityAnswerDistributionDTO> CandidateDistribution(string universityName) {
            var candidates = universityName == "Other" ?
                _context.Candidates.Where(c => !ValidUniversities.Contains(c.University!)).AsQueryable()
                :
                _context.Candidates.Where(c => c.University == universityName).AsQueryable();
            var disMap = new Dictionary<double, int>();
            await candidates.ForEachAsync(candidate => {
                var p = candidate.PercentageOfCorrectAnswers;
                if (disMap.ContainsKey(p)) disMap[p] = disMap[p] + 1;
                else disMap[p] = 1;
            });
            var tupList = new List<(double, int)>();
            foreach (var (rate, amount) in disMap)
            {
                tupList.Add((rate, amount));
            }
            tupList.Sort((tup1, tup2) => (int) (tup1.Item1 - tup2.Item1));

            var dto = new UniversityAnswerDistributionDTO() {
                Name = universityName,
                answerRates = tupList.Select(t => t.Item1).ToArray(),
                distribution = tupList.Select(t => t.Item2).ToArray()
            };

            return dto;
        }
    }
}
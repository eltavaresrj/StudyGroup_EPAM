namespace EPAM_WEBAPI.Tests.Engines
{
    using EPAM_WEBAPI.Application.DTO;
    using EPAM_WEBAPI.Tests;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Net.Mime;
    using System.Text;
    using System.Threading.Tasks;

    internal class StudyGroupActEngine
    {
        private TestWebApplicationFactory testWebApplicationFactory;
        public StudyGroupActEngine(TestWebApplicationFactory testWebApplicationFactory) 
        {
            this.testWebApplicationFactory = testWebApplicationFactory;
        }

        private const string getAllPath = "/v1/studyGroup/GetAll";
        private const string createPath = "/v1/studyGroup/Create";
        private const string joinPath = "/v1/studyGroup/Join";
        private const string RemovePath = "/v1/studyGroup/Remove";
        private const string GetBysubject = "/v1/studyGroup/GetBySubject";

        public async Task<List<StudyGroupResponseDto>> GetAllStudyGroups() 
        {            
            var response = await testWebApplicationFactory.Client.GetAsync(getAllPath);

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<StudyGroupResponseDto>>(content);

            return result;
        }

        public async Task<List<StudyGroupResponseDto>> GetBySubject(string subject)
        {
            var queryString = $"?subject={subject}";
            var response = await testWebApplicationFactory.Client.GetAsync(GetBysubject + queryString);

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<StudyGroupResponseDto>>(content);

            return result;
        }

        public async Task CreateStudyGroup(StudyGroupDto studyGroup)
        {
            var serializedStudyGroups = JsonConvert.SerializeObject(studyGroup);

            var stringContent = new StringContent(serializedStudyGroups, Encoding.UTF8, MediaTypeNames.Application.Json);
            
            await testWebApplicationFactory.Client.PostAsync(createPath, stringContent);
        }

        public async Task JoinStudyGroup(Guid studentId, Guid groupId)
        {
            var queryString = $"?studentId={studentId}&groupId={groupId}";
            
            await testWebApplicationFactory.Client.PostAsync(joinPath + queryString, null);
        }

        public async Task RemoveFromStudyGroup(Guid studentId, Guid groupId)
        {
            var queryString = $"?studentId={studentId}&groupId={groupId}";

            await testWebApplicationFactory.Client.PostAsync(RemovePath + queryString, null);
        }
    }
}

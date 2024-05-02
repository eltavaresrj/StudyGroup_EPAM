SELECT sg.*
FROM StudyGroup sg
INNER JOIN StudentStudyGroup ssg ON sg.Id = ssg.StudyGroupId
INNER JOIN Student s ON ssg.StudentId = s.Id
WHERE s.Name LIKE 'M%'
ORDER BY sg.CreatedDate;

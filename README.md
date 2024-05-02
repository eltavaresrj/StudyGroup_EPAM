# EPAM TEST - README


1.A & 1.B => I have decided to create a simple API implementing all the methods described in TestAppAPI, with that you can find below the test scenarios and inputs.

1.C => Following the Test Pyramid strategy, we will have most of the test scenarios implemented at the unit test level, some of the acceptence criteria implemented on a integration level, and a group of specific scenarios at the E2E level during UI manual tests.

1.D => Once the execution time is insignificant, I would consider all test scenarios for the regression tests.

2 => All features have at least one test scenario implemented

3 => Script

--------------------------------------

In order to have a code easy to test, I have decided to devide the API in the following layers:

1 - Controller ([StudyGroupController](https://github.com/eltavaresrj/EPAM_TEST/blob/master/EPAM_TEST/Controllers/StudyGroupController.cs) => [StudyGroupControllerTests](https://github.com/eltavaresrj/EPAM_TEST/blob/master/Tests/UnitTests/Controllers/StudyGroupControllerTests.cs))
2 - Application ([StudyGroupService](https://github.com/eltavaresrj/EPAM_TEST/blob/master/EPAM_TEST/Application/Services/StudyGroupService.cs) and [StudyGroupAdapter](https://github.com/eltavaresrj/EPAM_TEST/blob/master/EPAM_TEST/Application/Adapters/AdaptStudyGroup.cs) => [StudyGroupServicesTests](https://github.com/eltavaresrj/EPAM_TEST/blob/master/Tests/UnitTests/Application/Services/StudyGroupServicesTests.cs) and [AdaptStudyGroupTests](https://github.com/eltavaresrj/EPAM_TEST/blob/master/Tests/UnitTests/Application/Adapters/AdaptStudyGroupTests.cs))
3 - Model ([StudyGroup, Student and Subject](https://github.com/eltavaresrj/EPAM_TEST/tree/master/EPAM_TEST/Model))
4 - Repository ([StudyGroupRepository](https://github.com/eltavaresrj/EPAM_TEST/blob/master/EPAM_TEST/Repository/StudyGroup/StudyGroupRepository.cs) => [StudyGroupRepositoryTest](https://github.com/eltavaresrj/EPAM_TEST/blob/master/Tests/UnitTests/Repository/StudyGroupRepositoryTest.cs))

--------------------------------------

##StudyGroupFeature Test Scenarios

##[Unit Tests](https://github.com/eltavaresrj/EPAM_TEST/tree/master/Tests/UnitTests):

###1 - Controller [ Validate method response based on mocked service response ] - Ensure proper http response mapping

####- GetAll
======

 * Get Study Groups, Service returns Study Group List, Receive Status OK and List of Study Groups
 * Get Study Groups, Service returns empty study group list, Receive Status NotFound
 * Get Study Groups, Service returns null, Receive Status NotFound
 * Get Study Groups, Service returns exception, Receive Status Error with message "Error when trying to get Study Groups"
 ======

####- GetBySubject
======

 * Get StudyGroup by subject "Math", Service returns study group, Receive status OK and study group
 * Get StudyGroup by subject "Math", Service returns NO study group, Receive status NotFound
 * Get StudyGroup by subject "Math", Service returns null, Receive status NotFound
 * Get StudyGroup by subject "Math", Service returns exception, Receive Status Error with message "Error when trying to get Study Group"
 ======

####- Create
======

 * Create StudyGroup, Service returns OK, Receive status OK
 * Create StudyGroup, Service returns Exception, Receive status Error with error message "Study Group creation failed"
 * Create StudyGroup without Name, Receive BadRequest 
 * Create StudyGroup without Subject, Receive BadRequest
 ======

####- Join
======

 * Join StudyGroup, Service returns OK, Receive status OK
 * Join StudyGroup, Service returns Error, Receive status Error with message "Error when trying to join Study Group"
 * Join StudyGroup without StudyGroupId, Receive BadRequest
 * Join StudyGroup without StudentId, Receive BadRequest
======

####- Remove
======

 * Remove from StudyGroup, Service returns OK, Receive status OK
 * Remove from StudyGroup, Service returns Error, Receive status Error with message "Error when trying to remove from Study Group"
 * Remove from StudyGroup without StudyGroupId, Receive BadRequest
 * Remove from StudyGroup without StudentId, Receive BadRequest
 ======
 
###2 - Application :: Service [ Validate method response based on mocked repository and adapter responses ] - Ensure integrity between repository response and adapters consumptions

####- GetAllStudyGroupsAsync
======

 * Get Study Groups, Repository returns Study Group List, Adapter receives a List of Study Groups
 * Get Study Groups, Repository returns empty study group list, Adapter receives a empty list of Study Groups type
 * Get Study Groups, Repository returns null, Adapter receives null input
 * Get Study Groups, Repository returns exception, Method respond with exception
 ======

####- SearchStudyGroupBySubjectAsync
======

 * Search Study Group by subject Math, Repository returns Study Group, Adapter receives the Study Group
 * Search Study Group by subject Chemistry, Repository returns Study Group, Adapter receives the Study Group
 * Search Study Group by subject Physics, Repository returns Study Group, Adapter receives the Study Group
 * Search Study Group by subject PHYSICS, Repository returns Study Group, Adapter receives the Study Group
 * Search Study Group by subject Math, Repository returns NO Study group, Adapter receives a null input
 * Search Study Group by subject History, Error when trying to convert subject
 * Search Study Group by subject Math, Repository returns exception, Method respond with exception
======

####- CreateStudyGroupAsync
======

 * Create StudyGroup, Adapter returns StudyGroup, Repository receives StudyGroup
 * Create StudyGroup, Adapter returns null, Repository receives null
 ======

####- JoinUserToStudyGroupAsync
======

 * Join StudyGroup, repository returns OK, OK
 * Join StudyGroup, repository returns exception, thrown exception
======

####- RemoveUserFromStudyGroupAsync
======

  * Remove from StudyGroup, repository returns OK, OK
  * Remove from StudyGroup, repository returns exception, thrown exception
  ======

###3 - Application :: Adapter [ Validate method mapping from dto to domain and vice versa ] - Ensure proper mapping, adapting and casting rules

####- AdaptStudyGroupFromDtoToDomain
======

 * Adapt StudyGroupDto ("NameXpto", "Math"), Receive StudyGroup ("NameXpto", Math, CreatedDate)
 * Adapt StudyGroupDto ("NameXpto", "Chemistry"), Receive StudyGroup ("NameXpto", Math, CreatedDate)
 * Adapt StudyGroupDto ("NameXpto", "Physics"), Receive StudyGroup ("NameXpto", Physics, CreatedDate)
 * Adapt StudyGroupDto ("NameXpto", "History"), Receive Error "History is not a valida subject!"
 * Adapt StudyGroupDto ("NameXpto", "PHYSICS"), Receive StudyGroup ("NameXpto", Physics, CreatedDate)
 * Adapt StudyGroupDto ("Name", "Physics"), Receive Error "Name must have lenght between 5 and 30!"
 * Adapt StudyGroupDto ("!NameWithLenghHigherThan30Valid", "Physics"), Receive Error "Name must have lenght between 5 and 30!"
 * Adapt NULL, Receive NULL
 ======

 ####- AdaptStudyGroupsFromDomainToDto
 ======

 * Adapt List of StudyGroup with one ("NameXpto", Math, Students("1:S1","2:S2"), sysdate), Receive List of StudyGroupDto with One ("NameXpto", "Math", Students("S1","S2"), sysdate)
 * Adapt List of StudyGroup with multiple groups, Receive List of StudyGroupDto
 * Adapt NULL, Receive NULL
 * Adapt Empty List of StudyGroup, Receive Empty List of StudyGroupDto
 * Adapt List of StudyGroup with one StudyGroup without students, Receive List of StudyGroupDto with one StudyGroup without students
 * Adapt List of StudyGroup with one StudyGroup without name, Receive exception because name is required
 * Adapt List of StudyGroup with one StudyGroup without subject, Receive exception because subject is required
 ======

###4 - Repository [ Validate method response based on in memory database response ] - Ensure proper querying and database configurations

####- GetAll
======

 * Get Study Groups, dbContext returns Study Group List with student, Receive study groups with students
 * Get Study Groups, dbContext returns Study Group List without student, Receive study groups without students
 * Get Study Groups, dbContext returns empty study group list, Receive empty study group list
 * Get Study Groups, dbContext returns NULL, Receive NULL
 * Get Study Groups, dbContext returns exception, thrown exception
 ======

####- SearchStudyGroupBySubjectAsync
======

 * Get StudyGroup by subject "Math", dbContext returns TWO study groups (one with Math and one with Chemistry) with students, Receive Math study group only and with students
 * Get StudyGroup by subject "Math", dbContext returns TWO study groups (one with Math and one with Chemistry) without students, Receive Math study group only and without students
 * Get StudyGroup by subject "Math", dbContext returns NULL, Receive NULL
 * Get StudyGroup by subject "Math", dbContext returns null, Receive status NotFound
 * Get StudyGroup by subject "Math", dbContext returns exception, thrown exception
 ======

####- CreateStudyGroupAsync
======

 * Create StudyGroup, Receive OK
 * Create StudyGroup, dbContext returns Exception, thrown exception
 * Create StudyGroup without Name, thrown exception
 * Create StudyGroup without Subject, thrown exception
 * Create StudyGroup without Subject that already exist, thrown exception
 ======

####- JoinUserToStudyGroupAsync
======

 * Join StudyGroup, Receive OK
 * Join StudyGroup, dbContext returns Error, thrown exception
 * Join StudyGroup without StudyGroupId, thrown exception
 * Join StudyGroup without StudentId, thrown exception
 * Join StudyGroup with inexistent StudyGroupId, thrown exception
 * Join StudyGroup with inexistent StudentId, thrown exception
======

####- RemoveUserFromStudyGroupAsync
======

 * Remove from StudyGroup, Receive OK
 * Remove from StudyGroup, dbContext returns Error, thrown exception
 * Remove from StudyGroup without StudyGroupId, thrown exception
 * Remove from StudyGroup without StudentId, thrown exception
 * Remove from StudyGroup with inexistent StudyGroupId, thrown exception
 * Remove from StudyGroup with inexistent StudentId, thrown exception
 ======
 
 -----------------------------------------------
 
 ## [Integration Tests](https://github.com/eltavaresrj/EPAM_TEST/tree/master/Tests/IntegrationTests)
 
 With the API well splited and having the unit tests validating most of their responsibility, I would suggest the following 
 
 At this point we can ensure the whole api integration focusing only on the "Acceptence Criteria" test scenarios, implementing the following test cases on our automation project.
 
## TestCases:
 
### PreRequirements: (consider that after every test scenario the database is restored to the prerequeriment state)
 ======

 * Students already created
	- Gabriel Tavares : Id1234
	- Maria Paula : Id4567
	- Joao Dantas : Id7890
	
 * Study Groups already created
	- "ChemistryGroup", Chemistry, Students[Id4567, Id1234], DateXPTO
	
 * Subject limitation
	- Math
	- Chemistry
	- Physics
	======

1.0 - User Creates a Study Group ("MathStudyGroup", Math), System allow creation, StudyGroup created on database with createdDate equal sysdate.
1.1 - User Creates a Study Group ("PhysicsGroup", Physics), System allow creation, StudyGroup created on database
1.2 - User Creates a Study Group ("ChemistryGroup", Chemistry), System don't allow creation, "StudyGroup already exist"

2   - Student Id7890 Join "ChemistryGroup", system allow student to join, StudyGroup no have Students[Id4567, Id1234, Id7890]

2.1 - Student Id7890 Join "MathGroup", system don't allow student to join, StudyGroup "MathGroup" do not exist
2.2 - Student Id1111 Join "ChemistryGroup", system don't allow student to join, Student Id1111 do not exist
2.3 - Student Id1234 Join "ChemistryGroup", system don't allow student to join, Student already joined "ChemistryGroup"

3   - User search for all Study groups, system return list of study groups containing - "ChemistryGroup", Chemistry, Students[Id4567, Id1234], DateXPTO.
3.1 - User search for specific group by subject "Chemistry", system return list of study groups containing - "ChemistryGroup", Chemistry, Students[Id4567, Id1234], DateXPTO.
3.2 - User search for specific group by subject "Math", system return empty list

4 - Student Id1234 leave "ChemistryGroup", system allow removal, study group now has "ChemistryGroup", Chemistry, Students[Id4567], DateXPTO.


 ## Manual (E2E) tests:

* Assuming all previous tests were implemented, I would suggest to execute the rest of the Acceptence Criteria scenarios, plus a few UI validations:

5 - User Creates a Study Group ("Math", Math), System don't allow creation, "Name must have lenght between 5 and 30!"
5.1 - User Creates a Study Group ("!NameWithLenghHigherThan30Valid", Math), System don't allow creation, "Name must have lenght between 5 and 30!"
5.2 - User Creates a Study Group ("HistoryGroup", History), System don't allow creation, "History is not a valida subject!"

Assuming a complete system using this API wth an UI we should validate:

 - Authentication
 - Authorization
 - Browser compatibility
 - Mobile compatibility
 - Other bad requests
 - Security
 

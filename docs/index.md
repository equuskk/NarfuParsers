
# NarfuParsers
1. Расписание
	* Расписание групп студентов (`StudentsSchedule`)
		Пример использования (получить расписание у группы с ID 9000):
		```csharp
		var studentsSchedule = new StudentsSchedule();
		var studentsLessons = await studentsSchedule.GetLessons(9000);
		```
	* Расписание преподавателей (`TeachersSchedule`)
		Пример использования (получить расписание у преподавателя с ID 22900):
		```csharp
		var teacherSchedule = new TeachersSchedule();
		var teacherLessons = await teacherSchedule.GetLessons(22900);
		```
2. Парсеры
	* Парсер высших школ
		Пример использования (получить список всех высших школ):
		```csharp
		var schoolsParser = new SchoolsParser();
		var schools = await schoolsParser.GetSchools();
		```
	* Парсер групп из высшей школы
		Пример использования (получить список групп из высшей школы с ID 15):
		```csharp
		var groupsParser = new GroupsParser();
		var groups = await groupsParser.GetGroupsFromSchool(15);	
		```
	* Парсер преподавателей
		Пример использования (получить всех преподавателей в диапазоне от 22900 до 23000 с задержкой в 1000мс каждые 3 запроса):
		```csharp
		var teachersParser = new TeachersParser();
		var teachers = await teachersParser.GetTeachersInRange(22900, 23000, 3, 1000);	
		```
3. Использование proxy / переопределенного HttpClient
	Пример использования:
	```csharp
	const string ip = "1.1.1.1";
	const int port = 8080;
	const string proxyType = "http";
	var httpHandler = new HttpClientHandler
	{
		Proxy = new WebProxy(new Uri($"{proxyType}://{ip}:{port}")),
		Timeout = new TimeSpan(0, 0, 30)
	};
	var httpClient = new HttpClient(httpHandler)
	{
		BaseAddress = new Uri(Constants.EndPoint)
	};
	 
	var teachersParser = new TeachersParser(httpClient);
	var teachers = await teachersParser.GetTeachersInRange(21000, 23000, 10, 1000);
	```
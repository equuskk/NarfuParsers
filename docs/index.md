# NarfuParsers
## Расписание
* Расписание групп студентов (`StudentsSchedule`)  
	Пример использования (получить расписание у группы с ID 9000):
	```csharp
	var studentsSchedule = new StudentsSchedule(TimeSpan.FromSeconds(5));
	var studentsLessons = await studentsSchedule.GetLessons(9000);
	```
* Расписание преподавателей (`TeachersSchedule`)  
	Пример использования (получить расписание у преподавателя с ID 22900):
	```csharp
	var teacherSchedule = new TeachersSchedule(TimeSpan.FromSeconds(5));
	var teacherLessons = await teacherSchedule.GetLessons(22900);
	```

## Парсеры
* Парсер высших школ  
    Пример использования (получить список всех высших школ):
    ```csharp
    var schoolsParser = new SchoolsParser(TimeSpan.FromSeconds(5));
    var schools = await schoolsParser.GetSchools();
    ```
* Парсер групп из высшей школы  
    Пример использования (получить список групп из высшей школы с ID 15):
    ```csharp
    var groupsParser = new GroupsParser(TimeSpan.FromSeconds(5));
    var groups = await groupsParser.GetGroupsFromSchool(15);	
    ```
* Парсер преподавателей  
    Пример использования (получить всех преподавателей в диапазоне от 22900 до 23000 с задержкой в 1000мс каждые 3 запроса):
    ```csharp
    var teachersParser = new TeachersParser(TimeSpan.FromSeconds(5));
    var teachers = await teachersParser.GetTeachersInRange(22900, 23000, 3, 1000);	
    ```

## Сервисы
* Сервис для расписания  
    Пример использования
    ```csharp
    var service = new ScheduleService(TimeSpan.FromSeconds(5));
    var studentLessons = await service.Students.GetLessons(9000);
    var teacherLessons = await service.Teachers.GetLessons(22000);
    ```
* Сервис для парсинга  
    Пример использования
    ```csharp
    var service = new ParserService(TimeSpan.FromSeconds(5));
    var schools = (await service.Schools.GetSchools()).ToArray();
    var groups = await service.Groups.GetGroupsFromSchool(schools[0].Id);
    var teachers = await service.Teachers.GetTeachersInRange(22000, 23000, 5, 1500);
    ```

## Примеры использования библиотеки
* Примеры находятся здесь: [Примеры использования библиотеки](https://github.com/equuskk/NarfuParsers/tree/master/example/)
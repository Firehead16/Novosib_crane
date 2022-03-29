using System.Collections.Generic;
using Core.Global.Network;

public interface IAuthorizationDatabaseRequest: IRequest
{

    #region Работа с компьютерами

    /// <summary>
    /// Добавить компьютер
    /// </summary>	
    /// <param name="computer">Компьютер</param>
    void AddComp(Computer computer);

    /// <summary>
    /// Удалить компьютер
    /// </summary>
    /// <param name="computer">Компьютер</param>
    void DeleteComp(Computer computer);

    /// <summary>
    /// Существует ли компьютер
    /// </summary>
    bool IsCompExist(string compName);

    /// <summary>
    /// Получить компьютеры
    /// </summary>
    /// <returns></returns>
    List<Computer> GetComputers();

    /// <summary>
    /// Получить компьютер по имени
    /// </summary>
    /// <param name="compName"></param>
    /// <returns></returns>
    Computer GetComputerByComputerName(string compName);

    /// <summary>
    /// Получить компьютер по индексу
    /// </summary>
    /// <param name="computerId">Индекс компьютера</param>
    /// <returns></returns>
    Computer GetComputerByComputerId(int computerId);

    #endregion

    #region Работа с сессиями

    /// <summary>
    /// Добавить запись о названии задачи
    /// </summary>
    /// <param name="taskName">Имя задачи</param>
    /// <param name="sessionId">Индекс сессии</param>
    void SetTask(string taskName, int sessionId);

    /// <summary>
    /// Удалить запись о названии задачи
    /// </summary>
    /// <param name="sessionId">Индекс сессии</param>
    void DeleteTask(int sessionId);

    /// <summary>
    /// Добавить запись о прогрессе пользователя
    /// </summary>
    /// <param name="progressName">Прогресс</param>
    /// <param name="sessionId">Индекс сессии</param>
    void SetProgress(string progressName, int sessionId);

    /// <summary>
    /// Удалить запись о прогрессе пользователя
    /// </summary>
    /// <param name="sessionId"></param>
    void DeleteProgress(int sessionId);

    /// <summary>
    /// Открыть сессию
    /// </summary>
    /// <param name="person"> Пользователь</param>
    /// <param name="computer">Компьютер</param>
    Session OpenSession(Person person, Computer computer);

    /// <summary>
    /// Закрыть сессию
    /// </summary>
    /// <param name="session">Сессия</param>
    void CloseSession(Session session);

    /// <summary>
    /// Закрыть сессию
    /// </summary>
    /// <param name="computer">Компьютер</param>
    void CloseSession(Computer computer);

    /// <summary>
    /// Получить список сессий
    /// </summary>
    /// <returns></returns>
    List<Session> GetSessions();

    /// <summary>
    /// Получить индекс сессии у пользователя
    /// </summary>
    /// <param name="person">Пользователь</param>
    /// <returns></returns>
    Session GetSession(Person person);

    /// <summary>
    /// Получить индекс сессии у пользователя
    /// </summary>
    /// <param name="computer">Компьютер</param>
    /// <returns></returns>
    Session GetSession(Computer computer);

    /// <summary>
    /// Проверка открыта ли сессия на компьютере
    /// </summary>
    /// <param name="computer">Компьютер</param>
    /// <returns></returns>
    bool IsSessionOpen(Computer computer);

    /// <summary>
    /// Проверка открыта ли сессия у пользователя
    /// </summary>
    /// <param name="person">Пользователь</param>
    /// <returns></returns>
    bool IsSessionOpen(Person person);
    #endregion

    #region Работа с пользователями

    /// <summary>
    /// Добавить пользователя 
    /// </summary>
    /// <param name="person">Пользователь</param>
    void AddPerson(Person person);

    /// <summary>
    /// Редактировать пользователя
    /// </summary>
    /// <param name="person">Пользователь</param>
    void EditPerson(Person person);

    /// <summary>
    /// Удалить пользователя
    /// </summary>
    /// <param name="person">Пользователь</param>
    void DeletePerson(Person person);

    /// <summary>
    /// Получить пользователя
    /// </summary>
    /// <param name="personId">Индекс пользователя</param>
    /// <returns></returns>
    Person GetPersonByPersonId(int personId);

    /// <summary>
    /// Получить пользователя
    /// </summary>
    /// <param name="login">Логин</param>
    /// <param name="password">Пароль</param>
    /// <returns></returns>
    Person GetPerson(string login, string password);

    /// <summary>
    /// Получить список пользователей
    /// </summary>
    /// <returns></returns>
    List<Person> GetPersons();

    /// <summary>
    /// Проверка существования пользователя
    /// </summary>
    /// <param name="login">Логин</param>
    /// <param name="password">Пароль</param>
    /// <returns></returns>
    bool IsPersonExist(string login, string password);

    /// <summary>
    /// Проверка существования пользователя
    /// </summary>
    /// <param name="login"></param>
    /// <returns></returns>
    bool IsPersonExist(string login);

    /// <summary>
    /// Проверка открыта ли сессия у пользователя
    /// </summary>
    /// <param name="personId">Индекс пользователя</param>
    /// <returns></returns>
    bool IsPersonOpenSession(int personId);

    #endregion

    #region Работа с администраторами

    /// <summary>
    /// Добавить администратора
    /// </summary>
    /// <param name="administrator"></param>
    void AddAdministrator(Administrator administrator);

    /// <summary>
    /// Редактировать администратора
    /// </summary>
    /// <param name="administrator"></param>
    void EditAdministrator(Administrator administrator);

    /// <summary>
    /// Удалить администратора
    /// </summary>
    /// <param name="admin">Администратор</param>
    void DeleteAdministrator(Administrator admin);

    /// <summary>
    /// Получить администратора
    /// </summary>
    /// <param name="adminId">Индекс администратора</param>
    /// <returns></returns>
    Administrator GetAdministratorByAdminId(int adminId);

    /// <summary>
    /// Получить администратора 
    /// </summary>
    /// <param name="personId">Индекс пользователя</param>
    /// <returns></returns>
    Administrator GetAdministratorByPersonId(int personId);

    /// <summary>
    /// Получить список пользователей
    /// </summary>
    /// <returns></returns>
    List<Administrator> GetAdministrators();

    #endregion

    #region Работа с преподавателями

    /// <summary>
    /// Добавить преподавателя
    /// </summary>
    /// <param name="teacher"></param>
    void AddTeacher(Teacher teacher);

    /// <summary>
    /// Редактировать преподавателя
    /// </summary>
    /// <param name="teacher"></param>
    void EditTeacher(Teacher teacher);

    /// <summary>
    /// Удалить преподавателя
    /// </summary>
    /// <param name="teacher">Преподаватель</param>
    void DeleteTeacher(Teacher teacher);

    /// <summary>
    /// Получить преподавателя
    /// </summary>
    /// <param name="teacherId">Индекс преподавателя</param>
    /// <returns></returns>
    Teacher GetTeacherByTeacherId(int teacherId);


    /// <summary>
    /// Получить преподавателя 
    /// </summary>
    /// <param name="personId">Индекс пользователя</param>
    /// <returns></returns>
    Teacher GetTeacherByPersonId(int personId);

    /// <summary>
    /// Получить список преподавателей
    /// </summary>
    /// <returns></returns>
    List<Teacher> GetTeachers();

    #endregion

    #region Работа со студентами

    /// <summary>
    /// Добавить студента
    /// </summary>
    /// <param name="student"></param>
    void AddStudent(Student student);

    /// <summary>
    /// Редактировать студента
    /// </summary>
    /// <param name="student">Преподаватель</param>
    void EditStudent(Student student);

    /// <summary>
    /// Удалить студента
    /// </summary>
    /// <param name="student">Преподаватель</param>
    void DeleteStudent(Student student);

    /// <summary>
    /// Получить студента
    /// </summary>
    /// <param name="studentId">Индекс студента</param>
    /// <returns></returns>
    Student GetStudentByStudentId(int studentId);

    /// <summary>
    /// Получить студента
    /// </summary>
    /// <param name="personId">Индекс пользователя</param>
    /// <returns></returns>
    Student GetStudentByPersonId(int personId);

    /// <summary>
    /// Получить список студентов группы
    /// </summary>
    /// <param name="groupId"></param>
    /// <returns></returns>
    List<Student> GetStudentsByGroup(int groupId);

    /// <summary>
    /// Получить список студентов
    /// </summary>
    /// <returns></returns>
    List<Student> GetStudents();

    #endregion

    #region Работа с группами

    /// <summary>
    /// Добавить группу
    /// </summary>
    /// <param name="group">Группа</param>
    void AddGroup(Group group);

    /// <summary>
    /// Редактировать группу
    /// </summary>
    /// <param name="group">Группа</param>
    void EditGroup(Group group);

    /// <summary>
    /// Удалить группу
    /// </summary>
    /// <param name="group">Группа</param>
    void DeleteGroup(Group group);

    /// <summary>
    /// Получить группу
    /// </summary>
    /// <param name="groupId">Индекс группы</param>
    /// <returns></returns>
    Group GetGroupByGroupId(int? groupId);

    /// <summary>
    /// Получить группы по специальности
    /// </summary>
    /// <param name="speciality">Специальность</param>
    /// <returns></returns>
    List<Group> GetGroupsBySpeciality(Speciality speciality);

    /// <summary>
    /// Получить список групп
    /// </summary>
    /// <returns></returns>
    List<Group> GetGroups();

    #endregion

    #region Работа со специальностями

    /// <summary>
    /// Добавить специальность
    /// </summary>
    /// <param name="speciality">Специальность</param>
    void AddSpeciality(Speciality speciality);

    /// <summary>
    /// Редактировать специальность
    /// </summary>
    /// <param name="speciality">Специальность</param>
    void EditSpeciality(Speciality speciality);

    /// <summary>
    /// Удалить специальность
    /// </summary>
    /// <param name="speciality">Специальность</param>
    void DeleteSpeciality(Speciality speciality);

    /// <summary>
    /// Получить специальность
    /// </summary>
    /// <param name="specialityId">Индекс специальности</param>
    /// <returns></returns>
    Speciality GetSpecialityBySpecialityId(int specialityId);


    /// <summary>
    /// Получить список специальностей
    /// </summary>
    /// <returns></returns>
    List<Speciality> GetSpecialities();

    #endregion
}
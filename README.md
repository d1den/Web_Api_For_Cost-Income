# Web_Api_For_Cost-Income
## .Net Core Web Api application for manage Cost/Income
**Задача данной разработки:**  
ASP.NET CORE WEB-API веб-приложение для учёта расходов и доходов.  
Функциональные требования:  
1. Создание/редактирование/получение категорий доходов/расходов (Название категории).  
2. Создание/редактирование/получение расхода/дохода (категория, сумма, комментарий, дата - проставляется автоматически при создании записи).  
3. Просмотр отчёта доходы/расходы с группировкой по месяцам и/или категориям за указанный период.  
  
**Репозиторий включает в себя следующие папки:**  
1. Account - папка проекта;  
2. images - папка со скриншотами работы/теста программы.  
  
Программа типа Web-Api .Net Core разарботана на языке C#. На данный момент содержит только сервер, без клиента. В данный момент изучаю веб-разработку, htmll, css, js и в ближайшее время доделаю страницу-клиент. Сейчас же программа тестировалась с помощью приложения Postman.  
  
**Некоторые технические детали:**  
1. Программа работает со встраиваемой БД SQLite с помощью Entity.FrameworkCore. Реализована работа с БД подходом Code-First.  
2. Модели БД содержатся в папке Model.  
3. В папке Controllers содержатся контроллеры для взаимодействия с различными объектами: CategoryController - взаимодействие с категориями, CostController - взаимодействие с расходами, IncomeController - взаимодействие с доходами.  
4. Структура БД имеет следующий вид:  
Таблица с категориями доходов/расходов  
![table1](https://github.com/d1den/Web_Api_For_Cost-Income/blob/master/images/table1.JPG?raw=true "table1")  
Таблица с доходами/расходами, где помимо основных колонок имеется Type - тип записи (доход, расход):  
![table2](https://github.com/d1den/Web_Api_For_Cost-Income/blob/master/images/table2.JPG?raw=true "table2")  
5. Изначально приложение откроется по Url /category, выполнив Get запрос всех категорий;  
6. **При добавлении новго расхода/дохода проверяется наличие указанной категории в таблице категорий. Если категория уже существует, то берётся её Id, если нет, то создаётся новая категория.**  

**Возможности программы:**  
1. Get запросы для получения информации:
    + GET: category - получение всех КАТЕГОРИЙ доходов/расходов;  
    + GET: cost/bymonth - получение всех РАСХОДОВ с группировкой по меяцам;
    + GET: cost/bycategory/{periodStart}&{periodStop} - получение всех РАСХОДОВ с группировкой по категориям за указанный перид времени, где periodStart - дата начала периода в формате dd.mm.yyyy, а periodStop - дата конца периода;  
    + GET: cost/{id} - получение РАСХОДА по его Id;  
    + GET: income/bymonth - получение всех ДОХОДОВ с группировкой по меяцам;
    + GET: income/bycategory/{periodStart}&{periodStop} - получение всех ДОХОДОВ с группировкой по категориям за указанный перид времени, где periodStart - дата начала периода в формате dd.mm.yyyy, а periodStop - дата конца периода;  
    + GET: income/{id} - получение ДОХОДА по его Id.  
2. Post запросы для добавления новых данных:  
    + POST: category - добавление новой категории;
    + POST: cost - добавление нового расхода;
    + POST: income - добавление нового дохода.  
3. Put запросы для обновления данных:  
    + PUT: category - обновление данных о категории;  
    + PUT: cost - обновление данных о расходе;  
    + PUT: income - обновление данных о доходе.  
    
**Примеры работы программы:**  
1. Примеры выполенных Get запросов в браузере:  
![1](https://github.com/d1den/Web_Api_For_Cost-Income/blob/master/images/categoryGet.JPG?raw=true "1")  
![2](https://github.com/d1den/Web_Api_For_Cost-Income/blob/master/images/costByMonth.JPG?raw=true "2")  
![3](https://github.com/d1den/Web_Api_For_Cost-Income/blob/master/images/costByCategory1.JPG?raw=true "3")  
![4](https://github.com/d1den/Web_Api_For_Cost-Income/blob/master/images/costByCategory2.JPG?raw=true "4")  
Примеры с Income смотрите в папке [images](https://github.com/d1den/Web_Api_For_Cost-Income/blob/master/images/).  
2. Пример с Post и Put запросами. Изначальное состояние БД:  
![1](https://github.com/d1den/Web_Api_For_Cost-Income/blob/master/images/cat1.JPG?raw=true "1")  
![1](https://github.com/d1den/Web_Api_For_Cost-Income/blob/master/images/tr1.JPG?raw=true "1")  
**Добавление записи с новой категорией**  
![test1](https://github.com/d1den/Web_Api_For_Cost-Income/blob/master/images/test1.JPG?raw=true "test1")  
Новое состояние БД:  
![2](https://github.com/d1den/Web_Api_For_Cost-Income/blob/master/images/cat2.JPG?raw=true "2")  
![2](https://github.com/d1den/Web_Api_For_Cost-Income/blob/master/images/tr2.JPG?raw=true "2")  
**Обновление последней записи**  
![test2](https://github.com/d1den/Web_Api_For_Cost-Income/blob/master/images/test2.JPG?raw=true "test2")  
Новое состояние БД:  
![3](https://github.com/d1den/Web_Api_For_Cost-Income/blob/master/images/tr3.JPG?raw=true "3")  
**Добавление записи с уже имеющейся категорией (новой категории не добавилось)**  
![test3](https://github.com/d1den/Web_Api_For_Cost-Income/blob/master/images/test3.JPG?raw=true "test3")  
Новое состояние БД:  
![4](https://github.com/d1den/Web_Api_For_Cost-Income/blob/master/images/tr4.JPG?raw=true "4").

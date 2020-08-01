// Метод для добавления объекта строкой в таблицу
function AddRowToTable(info) {
    const tr = document.createElement("tr"); // Создаём новую строку
    tr.setAttribute("data-rowId", info.id);
    // Перебираем все свойства объекта
    for (var key in info) {
        if (key === "type") {
            continue;
        }
        const newTd = document.createElement("td"); // Создаём новую ячейку
        newTd.append(info[key]); // Добавляем к ней данные
        tr.append(newTd); // Добавляем её к строке
    }
    tr.append(CreateBtn(info["id"], "Редактировать", BtnUpdateValueClick, false));
    // Выводим строку
    return tr;
}

function ClearTable() {
    tb.innerHTML = "";
}

// Метод для удаления типа транзакции
function DeleteTypeFromTransaction(transactions){
    for (val of transactions){
        delete val["type"];
    }
}

// Метод для создания таблицы с полученным массивом заголовков
function CreateTable(columns) {
    ClearTable();
    // Создаём таблицу
    var table = document.createElement("table");
    var caption = document.createElement("caption");
    caption.textContent = typeInfo;
    table.append(caption);
    // Перебираем все элементы массива
    columns.forEach(column => {
        var th = document.createElement("th"); // Создаём заголвок
        th.append(column); // Добавляем название
        table.append(th); // Добавляем заголовок в общее заглавие
    });
    var th = document.createElement("th"); // Создаём заголвок
    table.append(th);
    tb.append(table); // Добавляем таблицу на форму
}

function ClearRbTypeGroup(){
    rbTypeGroup.innerHTML="";
}

function CreateInput(type, name){
    var input = document.createElement("input");
    input.type = type;
    input.name = name;
    return input;
}

function CreateRb(name, value){
    var rb = CreateInput("radio", name);
    rb.value = value;
    return rb;
}

function CreateTb(name, disabled){
    var tb = CreateInput("text", name);
    tb.disabled = disabled;
    return tb;
}

function CreateBtn(name, value, click, disabled){
    var btn = CreateInput("button", name);
    btn.value = value;
    btn.disabled = disabled;
    btn.addEventListener("click", click); 
    return btn;
}
function CreateLabel(forWhat, text){
    const label = document.createElement("label");
    label.setAttribute("for", forWhat);
    label.textContent = text;
    return label;
}

function BtByCategoryClick(e){
    const periodStart = rbTypeGroup.periodStart.value;
    const periodStop = rbTypeGroup.periodStop.value;
    if (rb.typeOfInfo[1].checked){
        GetIncomesByCategoryAndPeriod(periodStart, periodStop);
    }
    else if (rb.typeOfInfo[2].checked){
        GetCostsByCategoryAndPeriod(periodStart, periodStop);
    }
}

function RbTypeOfGrouppingClick(e){
    var type = e.target.value;
    if (type === "byMonth") {
        rbTypeGroup.periodStart.disabled = true;
        rbTypeGroup.periodStop.disabled = true;
        rbTypeGroup.Do.disabled = true;
        if (rb.typeOfInfo[1].checked) {
            GetIncomesByMonth();
        }
        else if (rb.typeOfInfo[2].checked) {
            GetCostsByMonth();
        }
    }
    else {
        rbTypeGroup.periodStart.disabled = false;
        rbTypeGroup.periodStop.disabled = false;
        rbTypeGroup.Do.disabled = false;
    }
}
function CreateRbTypeGroup() {
    ClearRbTypeGroup();
    var h3 = document.createElement("h3");
    h3.append("Тип группировки вывода");
    rbTypeGroup.append(h3);
    var rb1 = CreateRb("typeOfGroup", "byMonth");
    rb1.checked = true;
    rbTypeGroup.append(rb1);
    rbTypeGroup.append("По месяцам");
    rbTypeGroup.append(document.createElement("br"));
    rbTypeGroup.append(CreateRb("typeOfGroup", "byCategory"));
    rbTypeGroup.append("По категориям за указанный период (введите период в поля ниже в формате ##.##.####):");
    rbTypeGroup.append(document.createElement("br"));
    rbTypeGroup.append("С: ");
    rbTypeGroup.append(CreateTb("periodStart", true));
    rbTypeGroup.append(" По: ");
    rbTypeGroup.append(CreateTb("periodStop", true));
    rbTypeGroup.append(CreateBtn("Do", "Выполнить", BtByCategoryClick, true));
    for (var i=0; i<rbTypeGroup.typeOfGroup.length; i++){
        rbTypeGroup.typeOfGroup[i].addEventListener("click", RbTypeOfGrouppingClick);
    }
}
function BtnCreateValueClick(e) {
    const id = parseInt(addOrUpdateForm.id.value);
    if (rb.typeOfInfo[0].checked) {
        const name = addOrUpdateForm.inputCategory.value;
        if (id === 0){
            CreateCategory(name);
        }
        else {
            EditCategory(id, name);
            addOrUpdateForm.id.value = 0;
        }
    }
    else {
        const category = addOrUpdateForm.inputCategory.value;
        const sum = addOrUpdateForm.inputSum.value;
        const comment = addOrUpdateForm.inputComment.value;
        const date = addOrUpdateForm.inputDate.value;
        if (rb.typeOfInfo[1].checked){
            if (id === 0) {
                CreateTransaction("income", category, sum, comment);
            }
            else {
                EditTransaction("income", id, category, sum, comment, date);
                addOrUpdateForm.id.value = 0;
            }
        }
        else {
            if (id === 0) {
                CreateTransaction("cost", category, sum, comment);
            }
            else {
                EditTransaction("cost", id, category, sum, comment, date);
                addOrUpdateForm.id.value = 0;
            }
        }
        addOrUpdateForm.inputDate.disabled = true;
    }
    BtnResetValueClick();
}

function BtnResetValueClick(e) {
    var elements = addOrUpdateForm.elements;
    for (var i = 0; i < elements.length; i++){
        if (elements[i].type === "text"){
            elements[i].value = "";
        }
    }
}

function BtnUpdateValueClick(e) {
    if (rb.typeOfInfo[0].checked) {
        GetOneCategory(e.target.name);
    }
    else if (rb.typeOfInfo[1].checked) {
        GetOneIncome(e.target.name);
    }
    else {
        GetOneCost(e.target.name);
    }
}

function ClearInputForm(){
    addOrUpdateForm.innerHTML = "";
}

function CreateInputCategory() {
    ClearInputForm();
    var h3 = document.createElement("h3");
    h3.append("Создание/редактирование записи");
    addOrUpdateForm.append(h3);
    var id = CreateInput("hidden", "id");
    id.value = "0";
    addOrUpdateForm.append(id);
    addOrUpdateForm.append(CreateLabel("inputCategory", "Категория: "));
    var inputCategory = CreateInput("text", "inputCategory");
    addOrUpdateForm.append(inputCategory);
    addOrUpdateForm.append(document.createElement("br"));
    var createValueButton = CreateBtn("createBtn", "Создать", BtnCreateValueClick, false);
    addOrUpdateForm.append(createValueButton);
    var resetValueButton = CreateBtn("resetBtn", "Очистить", BtnResetValueClick, false);
    addOrUpdateForm.append(resetValueButton);
}

function CreateInputTransaction() {
    ClearInputForm();
    var h3 = document.createElement("h3");
    h3.append("Создание/редактирование записи");
    addOrUpdateForm.append(h3);
    var id = CreateInput("hidden", "id");
    id.value = "0";
    addOrUpdateForm.append(id);
    addOrUpdateForm.append(CreateLabel("inputCategory", "Категория: "));
    var inputCategory = CreateTb("inputCategory", false);
    addOrUpdateForm.append(inputCategory);
    addOrUpdateForm.append(document.createElement("br"));
    addOrUpdateForm.append(CreateLabel("inputSum", "Сумма: "));
    var inputSum = CreateTb("inputSum", false);
    addOrUpdateForm.append(inputSum);
    addOrUpdateForm.append(document.createElement("br"));
    addOrUpdateForm.append(CreateLabel("inputComment", "Комментарий: "));
    var inputComment = CreateTb("inputComment", false);
    addOrUpdateForm.append(inputComment);
    addOrUpdateForm.append(document.createElement("br"));
    addOrUpdateForm.append(CreateLabel("inputDate", "Дата: "));
    var inputDate = CreateTb("inputDate", true);
    addOrUpdateForm.append(inputDate);
    addOrUpdateForm.append(document.createElement("br"));
    var createValueButton = CreateBtn("createBtn", "Создать", BtnCreateValueClick, false);
    addOrUpdateForm.append(createValueButton);
    var resetValueButton = CreateBtn("resetBtn", "Очистить", BtnResetValueClick, false);
    addOrUpdateForm.append(resetValueButton);
}

async function GetSomeValues(stringUrl) {
    // отправляет запрос и получаем ответ
    const response = await fetch(stringUrl, {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    // если запрос прошел нормально
    if (response.ok === true) {
        // получаем данные
        const values = await response.json();
        ClearTable();
        CreateTable(cols);
        var table = document.querySelector("table");
        values.forEach(value => {
            // добавляем полученные элементы в таблицу
            table.append(AddRowToTable(value));
        });
    }
}

async function GetCategories() {
    GetSomeValues("/category");
}

async function GetIncomesByMonth() {
    GetSomeValues("/income/bymonth");
}

async function GetCostsByMonth() {
    GetSomeValues("/cost/bymonth");
}

async function GetIncomesByCategoryAndPeriod(periodStart, periodStop) {
    GetSomeValues("/income/bycategory/"+periodStart+"&"+periodStop);
}

async function GetCostsByCategoryAndPeriod(periodStart, periodStop) {
    GetSomeValues("/cost/bycategory/"+periodStart+"&"+periodStop);
}
async function GetOne(stringUrl) {
    // отправляет запрос и получаем ответ
    const response = await fetch(stringUrl, {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    // если запрос прошел нормально
    if (response.ok === true) {
        // получаем данные
        const value = await response.json();
        addOrUpdateForm.id.value = value.id;
        addOrUpdateForm.inputCategory.value = value.name;
        if (ObjectLength(value) > 2) {
            addOrUpdateForm.inputCategory.value = value.category;
            addOrUpdateForm.inputSum.value = value.sum;
            addOrUpdateForm.inputComment.value = value.comment;
            addOrUpdateForm.inputDate.value = value.date;
            addOrUpdateForm.inputDate.disabled = false;
        }
    }
}
function ObjectLength(thisObjeckt) {
    var count = 0;
    for (var key in thisObjeckt) {
        count++;
    }
    return count;
}
async function GetOneCategory(id) {
    GetOne("/category/" + id);
}
async function GetOneIncome(id) {
    GetOne("/income/" + id);
}
async function GetOneCost(id) {
    GetOne("/cost/" + id);
}


async function CreateCategory(nameCategory) {
    const response = await fetch("/category", {
        method: "POST",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            name: nameCategory
        })
    });
    if (response.ok === true) {
        const category = await response.json();
        var table = document.querySelector("table");
        table.append(AddRowToTable(category));
    }
}

async function CreateTransaction(theType, theCategory, theSum, theComment) {
    const response = await fetch("/"+theType, {
        method: "POST",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            category: theCategory,
            sum: parseFloat(theSum),
            comment: theComment
        })
    });
    if (response.ok === true) {
        const transaction = await response.json();
        var table = document.querySelector("table");
        table.append(AddRowToTable(transaction));
    }
}

async function EditCategory(theId, theName) {
    const response = await fetch("/category", {
        method: "PUT",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            id: parseInt(theId),
            name: theName
        })
    });
    if (response.ok === true) {
        const category = await response.json();
        document.querySelector("tr[data-rowid='" + category.id + "']").replaceWith(AddRowToTable(category));
    }
}

async function EditTransaction(theType, theId, theCategory, theSum, theComment, theDate) {
    const response = await fetch("/"+theType, {
        method: "PUT",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            id: parseInt(theId),
            category: theCategory,
            sum: parseFloat(theSum),
            comment: theComment,
            date: theDate
        })
    });
    if (response.ok === true) {
        const transaction = await response.json();
        document.querySelector("tr[data-rowid='" + transaction.id + "']").replaceWith(AddRowToTable(transaction));
    }
}


// Метод события нажатия на РБ
function RbTypeOfInfoClick(e) {
    var type = e.target.value;
    if (type === "Category") {
        CreateInputCategory();
        ClearRbTypeGroup();
        cols = ["Id", "Название"];
        typeInfo = "Категории";
        GetCategories();
    }
    else if (type === "Income") {
        CreateInputTransaction();
        CreateRbTypeGroup();
        cols = ["Id", "Категория", "Сумма", "Комментарий", "Дата"];
        typeInfo = "Доходы";
        GetIncomesByMonth();
    }
    else {
        CreateInputTransaction();
        CreateRbTypeGroup();
        cols = ["Id", "Категория", "Сумма", "Комментарий", "Дата"];
        typeInfo = "Расходы";
        GetCostsByMonth();
    }
}
// Главная часть программы
var cols = ["Id", "Название"];
var typeInfo = "Категории";
for (var i = 0; i < rb.typeOfInfo.length; i++) {
    rb.typeOfInfo[i].addEventListener("click", RbTypeOfInfoClick);
}
GetCategories();
CreateInputCategory();
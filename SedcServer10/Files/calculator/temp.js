function showPerson(person) {
    console.log(person.name);
}
// The C# wayu
var Person = /** @class */ (function () {
    function Person() {
    }
    return Person;
}());
var weko = new Person();
weko.name = "Wekoslav";
weko.age = 45;
showPerson(weko);
// The TypeScript way
var weko2 = {
    name: "Wekoslav",
    age: 45,
    lastName: "Stefanovski"
};
showPerson(weko2);

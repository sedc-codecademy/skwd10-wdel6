interface IPerson {
    name: string;
    age: number;
}

function showPerson(person: IPerson): void {
    console.log(person.name);
}

// The C# wayu
class Person implements IPerson {
    name: string = ""
    age: number = 0;
}

const weko = new Person();
weko.name = "Wekoslav";
weko.age = 45;

showPerson(weko);

// The TypeScript way
const weko2 = {
    name: "Wekoslav",
    age: 45,
    lastName: "Stefanovski"
};

showPerson(weko2);
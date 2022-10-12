const calculate = async () => {
    const first = document.getElementById('first').valueAsNumber;
    const second = document.getElementById('second').valueAsNumber;
    const operator = document.getElementById('operation').value;

    const result = await getResult(first, second, operator);
    document.getElementById('result').value = result.result;
}

document.addEventListener('DOMContentLoaded', function() {
    document.getElementById('calculate').addEventListener('click', calculate);
});

const getResult = async (first, second, operator) => {
    const response = await fetch(`/api/calculate/${operator}/${first}/${second}`);
    const result = await response.json();
    return result;
}
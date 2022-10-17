const calculate = async () => {
    const first = document.getElementById('first').valueAsNumber;
    const second = document.getElementById('second').valueAsNumber;
    const operator = document.getElementById('operation').value;

    const result = await getResult(first, second, operator);

    const response = (result.success)
     ? `${result.first} ${result.operation.symbol} ${result.second} = ${result.result}`
     : `Error occured while calculating: ${result.first} ${result.operation.symbol} ${result.second}`;

    document.getElementById('result').innerHTML = response;
}

document.addEventListener('DOMContentLoaded', function() {
    document.getElementById('calculate').addEventListener('click', calculate);
});

const getResult = async (first, second, operator) => {
    const response = await fetch(`/api/calculate/${operator}/${first}/${second}`);
    const result = await response.json();
    return result;
}
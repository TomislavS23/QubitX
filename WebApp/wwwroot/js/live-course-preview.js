function display()
{
    const userInput = document.getElementById("textbox").value
    const outputDiv = document.getElementById("resultdiv")
    const parsedResult = marked.parse(userInput)
    outputDiv.innerHTML = parsedResult;
    hljs.highlightAll();
}

document.getElementById('textbox').addEventListener('keydown', function(e) {
    if (e.key == 'Tab') {
        e.preventDefault();
        var start = this.selectionStart;
        var end = this.selectionEnd;

        // set textarea value to: text before caret + tab + text after caret
        this.value = this.value.substring(0, start) +
            "    " + this.value.substring(end);

        // put caret at right position again
        this.selectionStart =
            this.selectionEnd = start + 4;
    }
    display();
});

document.getElementById('textbox').addEventListener('input', function(e)
{
    display();
});

window.onload = function()
{
    display();
}

function reset()
{
    display();
    console.log("Reset")
}
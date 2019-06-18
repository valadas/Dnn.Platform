document.addEventListener("DOMContentLoaded", function(){
    const wrappers = document.querySelectorAll(".dnn-dam");
    const heading = document.createElement("h1");
    heading.innerText = "Hello from javascript";
    
    wrappers.forEach(function(wrapper){
        wrapper.appendChild(heading);
    });

});

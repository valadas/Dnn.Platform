document.addEventListener("DOMContentLoaded", (): void => {
    const wrappers = document.querySelectorAll(".dnn-dam");
    const heading = document.createElement("h1");
    
    wrappers.forEach((wrapper: HTMLElement): void => {
        const moduleId = parseInt(wrapper.dataset.moduleId);
        heading.innerText = "The module ID is: " + moduleId;
        wrapper.appendChild(heading);
    });
});

import * as React from 'react';
import * as ReactDom from 'react-dom';
import App from './App';

document.addEventListener("DOMContentLoaded", (): void => {
    const wrappers = document.querySelectorAll(".dnn-dam");
    
    wrappers.forEach((wrapper: HTMLElement): void => {
        const moduleId = parseInt(wrapper.dataset.moduleId);
        
        ReactDom.render(<App moduleId={moduleId} />, wrapper);
    });
});

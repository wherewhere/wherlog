﻿// Add Stylesheets
hljs_addStylesheet('https://cdn.jsdelivr.net/npm/@highlightjs/cdn-assets/styles/vs.min.css', 'highlight-light', null);
hljs_addStylesheet('https://cdn.jsdelivr.net/npm/@highlightjs/cdn-assets/styles/vs2015.min.css', 'highlight-dark', 'disabled');

hljs_addInlineStylesheet(`pre[class~="snippet"] {
    --type-ramp-base-font-variations: unset;
    font-weight: bold;
    }`);

// Add Scripts
const highlight = hljs_addJavaScript('https://cdn.jsdelivr.net/npm/@highlightjs/cdn-assets/highlight.min.js');

// Add custom code
highlight.onload = () => {
    // Add languages
    hljs_addLanguages(
        'dos',
        'powershell'
    );
    hljs_addJavaScript('js/highlight-msil.js');

    // Switch highlight Dark/Light theme
    const theme = document.querySelector('fluent-design-theme');
    if (theme != null) {
        theme.addEventListener('onchange', e => {
            if (e.detail.name == 'mode') {
                if (e.detail.newValue === 'undefined') { return; }
                const isDark = e.detail.newValue.includes('dark');
                hljs_ColorSwitcher(isDark);
            }
        });
    }

    // Detect system theme changing
    window.matchMedia('(prefers-color-scheme: dark)')
          .addEventListener('change', _ => hljs_ColorSystem());

    // First/default theme
    hljs_ColorSystem();
}

function hljs_ColorSystem() {
    const theme = document.querySelector('fluent-design-theme');
    if (theme != null) {
        if (theme.getAttribute('mode') == 'null' || theme.getAttribute('mode') == null || theme.getAttribute('mode').value === undefined) {
            const isSystemDark = window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;
            hljs_ColorSwitcher(isSystemDark);
        }
    }
}

function hljs_ColorSwitcher(isDark) {
    const darkCss = document.querySelector('link[title="highlight-dark"]');
    const lightCss = document.querySelector('link[title="highlight-light"]');

    if (isDark) {
        darkCss.removeAttribute("disabled");
        lightCss.setAttribute("disabled", "disabled");
    }
    else {
        lightCss.removeAttribute("disabled");
        darkCss.setAttribute("disabled", "disabled");
    }
}

function hljs_addLanguages(...langs) {
    langs.forEach(lang => hljs_addJavaScript(`https://cdn.jsdelivr.net/npm/@highlightjs/cdn-assets/languages/${lang}.min.js`));
}

// Add a <script> to the <body> element
function hljs_addJavaScript(src) {
    const script = document.createElement('script');
    script.type = 'text/javascript';
    script.src = src;
    script.async = true;

    script.onerror = () => {
        // Error occurred while loading script
        console.error('Error occurred while loading script', src);
    };

    document.body.appendChild(script);

    return script;
}

// Add a <link> to the <head> element
function hljs_addStylesheet(src, title, disabled) {
    const stylesheet = document.createElement('link');
    stylesheet.rel = 'stylesheet';
    stylesheet.href = src;
    if (title) stylesheet.title = title;
    if (disabled) stylesheet.disabled = disabled;

    stylesheet.onerror = () => {
        // Error occurred while loading stylesheet
        console.error('Error occurred while loading stylesheet', src);
    };

    document.head.appendChild(stylesheet);

    return stylesheet;
}

function hljs_addInlineStylesheet(code) {
    const stylesheet = document.createElement('style');
    stylesheet.innerText = code;

    document.head.appendChild(stylesheet);

    return stylesheet;
}

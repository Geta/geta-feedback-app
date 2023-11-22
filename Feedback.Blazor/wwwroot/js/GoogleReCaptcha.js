function googleReCaptcha(dotNetObjectReference, htmlID, siteKeyValue) {
    return window.grecaptcha.render(htmlID, {
        'sitekey': siteKeyValue,
        'callback': (response) => { dotNetObjectReference.invokeMethodAsync('CallbackOnSuccess', response); }
    });
};

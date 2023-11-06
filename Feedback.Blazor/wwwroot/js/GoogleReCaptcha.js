function googleReCaptcha(dotNetObjectReference, htmlID, siteKeyValue) {
    return grecaptcha.render(htmlID, {
        'sitekey': siteKeyValue,
        'callback': (response) => { dotNetObjectReference.invokeMethodAsync('CallbackOnSuccess', response); }
    });
};

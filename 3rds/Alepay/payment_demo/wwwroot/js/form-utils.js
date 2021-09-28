// Form utils
// Copyright 2021 @maithutrading

// Constants
const WC_ORIGINAL_VALUE_ATTR_NAME = 'data-wc-original-value';
const WC_DEFAULT_VALUE_ATTR_NAME = 'data-initial-value';

// Name of the control has value is indicate warn changed feature enabled state of current form
const WC_CONTROL_NAME = 'warnChanged';

function getFormData(selector) {
    var dialog = $(selector);
    var dialogData = {};
    var childControls = dialog.find('input.use-fu, select.use-fu, textarea.use-fu');
    childControls.each(function () {
        var controlName = $(this).attr('name');
        if (dialogData[controlName] != undefined)
            return;

        // WarnChanged supported
        if (controlName == WC_CONTROL_NAME)
            return;
        if (controlName == null)
            return;
        if ($(this).is('input[type="checkbox"]')) {
            dialogData[controlName] = $(this).is(':checked');
        } else {
            dialogData[controlName] = $(this).val();
        }
    });
    return dialogData;
}

function resetForm(selector) {
    var dialog = $(selector);
    var controls = $(dialog).find('input.use-fu, select.use-fu, textarea.use-fu');
    if (controls == null)
        return;
    controls.each(function () {
        if ($(this).attr('name') == WC_CONTROL_NAME)
            return;
        if (this.hasAttribute(WC_DEFAULT_VALUE_ATTR_NAME)) {
            var defaultValue = $(this).attr(WC_DEFAULT_VALUE_ATTR_NAME);
            $(this).val(defaultValue);
        }
        $(this).attr(WC_ORIGINAL_VALUE_ATTR_NAME, '');
    });
}

function restoreFormData(selector, data) {
    var dialog = $(selector);
    for (var p in data) {
        // find control case insensitive
        var controls = dialog.find('input.use-fu, select.use-fu, textarea.use-fu, span.readonly-input');
        if (!controls || controls.length <= 0) {
            console.warn('[FormUtils::restoreFormData] No input in this selection');
            return;
        }
        var matchedControls = [];
        $(controls).each(function (i, c) {
            var control = $(c);
            var controlName = undefined;
            if (control.is('span.readonly-input'))
                controlName = control.attr('data-name');
            else
                controlName = control.attr('name');
            if (!controlName || controlName.length <= 0)
                return;
            if (controlName.toLowerCase() == p.toLowerCase())
                matchedControls.push(control);
        });
        if (!matchedControls || matchedControls.length <= 0) {
            console.warn('[FormUtils::restoreFormData] No control named "' + p + '"');
            continue;
        }
        matchedControls.forEach(function (control) {
            var value = data[p];
            // input checkbox type
            if (control.is('input[type="checkbox"]')) {
                control.prop('checked', value);
            // input is readonly (span.readonly-input)
            } else if(control.is('span.readonly-input')) {
                control.text(value);
            // other types
            } else {
                control.val(value);
            }
    
            // Set original value
            if (!control.is('span.readonly-input'))
                control.attr(WC_ORIGINAL_VALUE_ATTR_NAME, value);
        });

    }
}
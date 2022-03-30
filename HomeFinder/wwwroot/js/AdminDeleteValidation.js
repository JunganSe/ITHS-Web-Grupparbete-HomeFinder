function confirmDelete(uniqueId, isTrue) {

    var deleteSpan = 'deleteSpan' + uniqueId;
    var confirmDeleteSpan = 'confirmDeleteSpan' + uniqueId;

    if (isTrue) {
        $('#' + deleteSpan).hide();
        $('#' + confirmDeleteSpan).show();
    } else {
        $('#' + deleteSpan).show();
        $('#' + confirmDeleteSpan).hide();
    }
}
window.kts = (function (kts, $, undefined) {
    
    kts.editPage = (function ($, undefined) {
        var addNewSectionButton = $("#add-new-section");
        var editForm = $("#book-edit-form");
        var bookSections = $("#book-sections");

        var addNewSection = function () {
            var serializedFormData = editForm.serialize();

            $.ajax({
                data: serializedFormData,
                dataType: "html",
                method: "POST",
                success: function (data, status, xhr) {
                    bookSections.html(data);
                    addClickToSectionButtons();
                },
                url: "/Books/AddSection"
            });
        };

        var removeSection = function (sectionToRemove) {
            var currentSection = sectionToRemove;
            var nextSection = $(currentSection).next().next(".book-section");

            while (nextSection.length > 0) {
                swapSectionField(currentSection, nextSection[0], "Title");
                swapSectionField(currentSection, nextSection[0], "Content", true);

                currentSection = nextSection[0];
                nextSection = nextSection.next().next(".book-section");
            }

            $(currentSection).next().remove();
            $(currentSection).remove();
        };

        var moveSection = function (bookSection, isMovingUp) {

            var sectionToSwitchWith = isMovingUp === true ? $(bookSection).prev().prev(".book-section") : $(bookSection).next().next(".book-section");

            // if there is no section to switch with then leave this function
            if (sectionToSwitchWith.length <= 0) {
                return;
            }
            
            swapSectionField(bookSection, sectionToSwitchWith[0], "Title");
            swapSectionField(bookSection, sectionToSwitchWith[0], "Content", true);
        };

        var addClickToSectionButtons = function () {
            $(".book-section [data-book-section='up-button']").click(function (arg) {
                moveSection($(this).parents(".book-section")[0], true);
            });

            $(".book-section [data-book-section='down-button']").click(function (arg) {
                moveSection($(this).parents(".book-section")[0], false);
            });

            $(".book-section [data-book-section='remove-button']").click(function (arg) {
                removeSection($(this).parents(".book-section")[0]);
            });
        };

        var swapSectionField = function (srcSection, destSection, fieldName, isKendoEditor) {
            var srcFieldValue = $("[name$='" + fieldName + "']", srcSection).val();
            var destFieldValue = $("[name$='" + fieldName + "']", destSection).val();
            $("[name$='" + fieldName + "']", srcSection).val(destFieldValue);
            $("[name$='" + fieldName + "']", destSection).val(srcFieldValue);

            if (isKendoEditor === true) {
                $("[name$='" + fieldName + "']", srcSection).data("kendoEditor").value(destFieldValue);
                $("[name$='" + fieldName + "']", destSection).data("kendoEditor").value(srcFieldValue);
            }
        }

        var initialize = function () {
            addNewSectionButton.click(function (arg) {
                addNewSection();
            });

            addClickToSectionButtons();
        };

        return {
            intialize: initialize
        }
    })($, undefined);

    return kts;
})(window.kts || {}, jQuery);


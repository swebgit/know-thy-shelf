﻿window.kts = (function (kts, $, undefined) {
    
    kts.editPage = (function ($, undefined) {
        var addNewSectionButton = $("#add-new-section");
        var editForm = $("#book-edit-form");
        var bookSections = $("#book-sections");

        var titleField = $("#Title");
        var searchTitleField = $("#SearchTitle");

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

        var copyTitleToSearchTitle = function () {
            var titleValue = titleField.val();
            wordsToIgnoreRegex = /^(a\s|an\s|the\s)/i;
            var regexMatches = titleValue.match(wordsToIgnoreRegex);
            if (regexMatches !== null && regexMatches.length > 0) {
                searchTitleField.val(titleValue.replace(regexMatches[0], ""));
            } else {
                searchTitleField.val(titleValue);
            }
        };

        var addOnChangeToTitleField = function () {
            titleField.keyup(function () {
                copyTitleToSearchTitle();
            });

            titleField.change(function () {
                copyTitleToSearchTitle();
            });
        };

        var initialize = function () {
            addNewSectionButton.click(function (arg) {
                addNewSection();
            });

            addClickToSectionButtons();

            addOnChangeToTitleField();
        };

        var saveImageUrl = function (inputSelector, imageSelector, url) {
            $(inputSelector).val(url);
            $(imageSelector).attr('src', url);
            $(imageSelector).removeClass('hidden');

        };

        return {
            intialize: initialize,
            saveImageUrl: saveImageUrl
        }
    })($, undefined);

    return kts;
})(window.kts || {}, jQuery);


$(function () {
    var l = abp.localization.getResource('ErmanApp');
    var countryService = vumbaSoft.ermanApp.demographics.countries.country;
    var createModal = new abp.ModalManager(abp.appPath + 'Demographics/Countries/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'Demographics/Countries/EditModal');
    var dataTable = $('#CountriesTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(countryService.getList),
            columnDefs: [
                { title: l('Actions'), rowAction: { items: [
                    { text: l('Edit'), visible: abp.auth.isGranted('ErmanApp.Countries.Edit'), action: function (data) { editModal.open({ id: data.record.id }); } },
                    { text: l('Delete'), visible: abp.auth.isGranted('ErmanApp.Countries.Delete'), confirmMessage: function (data) { return l('CountryDeletionConfirmationMessage', data.record.name); }, action: function (data) { countryService.delete(data.record.id).then(function() { abp.notify.success(l('DeletedSuccessfully')); dataTable.ajax.reload(); }); } }
                ] } },
                { title: l('Name'), data: "name" },
                { title: l('Region'), data: "regionName" },
                { title: l('Population'), data: "population" },
                { title: l('FormalName'), data: "formalName" },
                { title: l('NativeName'), data: "nativeName" },
                { title: l('ISO3'), data: "isO3" },
                { title: l('ISO2'), data: "isO2" },
                { title: l('CCN3'), data: "ccN3" },
                { title: l('PhoneCode'), data: "phoneCode" },
                { title: l('Capital'), data: "capital" },
                { title: l('Currency'), data: "currency" },
                { title: l('Emoji'), data: "emoji" },
                { title: l('EmojiU'), data: "emojiU" },
                { title: l('Remarks'), data: "remarks" },
                { title: l('CreationTime'), data: "creationTime", dataFormat: "datetime" }
            ]
        })
    );
    createModal.onResult(function () { abp.notify.success(l('CreatedSuccessfully')); dataTable.ajax.reload(); });
    editModal.onResult(function () { abp.notify.success(l('SavedSuccessfully')); dataTable.ajax.reload(); });
    $('#NewCountryButton').click(function (e) { e.preventDefault(); createModal.open(); });
});

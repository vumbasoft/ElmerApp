$(function () {
    var l = abp.localization.getResource('ErmanApp');
    var continentService = vumbaSoft.ermanApp.demographics.continents.continent;
    var createModal = new abp.ModalManager(abp.appPath + 'Demographics/Continents/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'Demographics/Continents/EditModal');
    var dataTable = $('#ContinentsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(continentService.getList),
            columnDefs: [
                { title: l('Actions'), rowAction: { items: [
                    { text: l('Edit'), visible: abp.auth.isGranted('ErmanApp.Continents.Edit'), action: function (data) { editModal.open({ id: data.record.id }); } },
                    { text: l('Delete'), visible: abp.auth.isGranted('ErmanApp.Continents.Delete'), confirmMessage: function (data) { return l('ContinentDeletionConfirmationMessage', data.record.name); }, action: function (data) { continentService.delete(data.record.id).then(function() { abp.notify.success(l('DeletedSuccessfully')); dataTable.ajax.reload(); }); } }
                ] } },
                { title: l('Name'), data: "name" },
                { title: l('Population'), data: "population" },
                { title: l('Remarks'), data: "remarks" },
                { title: l('CreationTime'), data: "creationTime", dataFormat: "datetime" }
            ]
        })
    );
    createModal.onResult(function () { abp.notify.success(l('CreatedSuccessfully')); dataTable.ajax.reload(); });
    editModal.onResult(function () { abp.notify.success(l('SavedSuccessfully')); dataTable.ajax.reload(); });
    $('#NewContinentButton').click(function (e) { e.preventDefault(); createModal.open(); });
});

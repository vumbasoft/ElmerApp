$(function () {
    var l = abp.localization.getResource('ErmanApp');
    var subcontinentService = vumbaSoft.ermanApp.demographics.subcontinents.subcontinent;
    var createModal = new abp.ModalManager(abp.appPath + 'Demographics/Subcontinents/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'Demographics/Subcontinents/EditModal');
    var dataTable = $('#SubcontinentsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(subcontinentService.getList),
            columnDefs: [
                { title: l('Actions'), rowAction: { items: [
                    { text: l('Edit'), visible: abp.auth.isGranted('ErmanApp.Subcontinents.Edit'), action: function (data) { editModal.open({ id: data.record.id }); } },
                    { text: l('Delete'), visible: abp.auth.isGranted('ErmanApp.Subcontinents.Delete'), confirmMessage: function (data) { return l('SubcontinentDeletionConfirmationMessage', data.record.name); }, action: function (data) { subcontinentService.delete(data.record.id).then(function() { abp.notify.success(l('DeletedSuccessfully')); dataTable.ajax.reload(); }); } }
                ] } },
                { title: l('Name'), data: "name" },
                { title: l('Continent'), data: "continentName" },
                { title: l('Population'), data: "population" },
                { title: l('Remarks'), data: "remarks" },
                { title: l('CreationTime'), data: "creationTime", dataFormat: "datetime" }
            ]
        })
    );
    createModal.onResult(function () { abp.notify.success(l('CreatedSuccessfully')); dataTable.ajax.reload(); });
    editModal.onResult(function () { abp.notify.success(l('SavedSuccessfully')); dataTable.ajax.reload(); });
    $('#NewSubcontinentButton').click(function (e) { e.preventDefault(); createModal.open(); });
});

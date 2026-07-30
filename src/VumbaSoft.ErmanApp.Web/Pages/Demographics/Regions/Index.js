$(function () {
    var l = abp.localization.getResource('ErmanApp');
    var regionService = vumbaSoft.ermanApp.demographics.regions.region;
    var createModal = new abp.ModalManager(abp.appPath + 'Demographics/Regions/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'Demographics/Regions/EditModal');
    var dataTable = $('#RegionsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(regionService.getList),
            columnDefs: [
                { title: l('Actions'), rowAction: { items: [
                    { text: l('Edit'), visible: abp.auth.isGranted('ErmanApp.Regions.Edit'), action: function (data) { editModal.open({ id: data.record.id }); } },
                    { text: l('Delete'), visible: abp.auth.isGranted('ErmanApp.Regions.Delete'), confirmMessage: function (data) { return l('RegionDeletionConfirmationMessage', data.record.name); }, action: function (data) { regionService.delete(data.record.id).then(function() { abp.notify.success(l('DeletedSuccessfully')); dataTable.ajax.reload(); }); } }
                ] } },
                { title: l('Name'), data: "name" },
                { title: l('Subcontinent'), data: "subcontinentName" },
                { title: l('Population'), data: "population" },
                { title: l('Remarks'), data: "remarks" },
                { title: l('CreationTime'), data: "creationTime", dataFormat: "datetime" }
            ]
        })
    );
    createModal.onResult(function () { abp.notify.success(l('CreatedSuccessfully')); dataTable.ajax.reload(); });
    editModal.onResult(function () { abp.notify.success(l('SavedSuccessfully')); dataTable.ajax.reload(); });
    $('#NewRegionButton').click(function (e) { e.preventDefault(); createModal.open(); });
});

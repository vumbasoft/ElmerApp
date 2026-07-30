$(function () {
    var l = abp.localization.getResource('ErmanApp');
    var stateProvinceService = vumbaSoft.ermanApp.demographics.stateProvinces.stateProvince;
    var createModal = new abp.ModalManager(abp.appPath + 'Demographics/StateProvinces/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'Demographics/StateProvinces/EditModal');
    var dataTable = $('#StateProvincesTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(stateProvinceService.getList),
            columnDefs: [
                { title: l('Actions'), rowAction: { items: [
                    { text: l('Edit'), visible: abp.auth.isGranted('ErmanApp.StateProvinces.Edit'), action: function (data) { editModal.open({ id: data.record.id }); } },
                    { text: l('Delete'), visible: abp.auth.isGranted('ErmanApp.StateProvinces.Delete'), confirmMessage: function (data) { return l('StateProvinceDeletionConfirmationMessage', data.record.name); }, action: function (data) { stateProvinceService.delete(data.record.id).then(function() { abp.notify.success(l('DeletedSuccessfully')); dataTable.ajax.reload(); }); } }
                ] } },
                { title: l('Name'), data: "name" },
                { title: l('Country'), data: "countryName" },
                { title: l('Population'), data: "population" },
                { title: l('RegionCode'), data: "regionCode" },
                { title: l('StateProvinceCode'), data: "stateProvinceCode" },
                { title: l('Remarks'), data: "remarks" },
                { title: l('CreationTime'), data: "creationTime", dataFormat: "datetime" }
            ]
        })
    );
    createModal.onResult(function () { abp.notify.success(l('CreatedSuccessfully')); dataTable.ajax.reload(); });
    editModal.onResult(function () { abp.notify.success(l('SavedSuccessfully')); dataTable.ajax.reload(); });
    $('#NewStateProvinceButton').click(function (e) { e.preventDefault(); createModal.open(); });
});

$(function () {
    var l = abp.localization.getResource('ErmanApp');
    var localityService = vumbaSoft.ermanApp.demographics.localities.locality;
    var createModal = new abp.ModalManager(abp.appPath + 'Demographics/Localities/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'Demographics/Localities/EditModal');
    var dataTable = $('#LocalitiesTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(localityService.getList),
            columnDefs: [
                { title: l('Actions'), rowAction: { items: [
                    { text: l('Edit'), visible: abp.auth.isGranted('ErmanApp.Localities.Edit'), action: function (data) { editModal.open({ id: data.record.id }); } },
                    { text: l('Delete'), visible: abp.auth.isGranted('ErmanApp.Localities.Delete'), confirmMessage: function (data) { return l('LocalityDeletionConfirmationMessage', data.record.name); }, action: function (data) { localityService.delete(data.record.id).then(function() { abp.notify.success(l('DeletedSuccessfully')); dataTable.ajax.reload(); }); } }
                ] } },
                { title: l('Name'), data: "name" },
                { title: l('DistrictCity'), data: "districtCityName" },
                { title: l('Population'), data: "population" },
                { title: l('DistrictCityCode'), data: "districtCityCode" },
                { title: l('LocalityCode'), data: "localityCode" },
                { title: l('Latitude'), data: "latitude" },
                { title: l('Longitude'), data: "longitude" },
                { title: l('Remarks'), data: "remarks" },
                { title: l('CreationTime'), data: "creationTime", dataFormat: "datetime" }
            ]
        })
    );
    createModal.onResult(function () { abp.notify.success(l('CreatedSuccessfully')); dataTable.ajax.reload(); });
    editModal.onResult(function () { abp.notify.success(l('SavedSuccessfully')); dataTable.ajax.reload(); });
    $('#NewLocalityButton').click(function (e) { e.preventDefault(); createModal.open(); });
});

$(function () {
    var l = abp.localization.getResource('ErmanApp');
    var districtCityService = vumbaSoft.ermanApp.demographics.districtCities.districtCity;
    var createModal = new abp.ModalManager(abp.appPath + 'Demographics/DistrictCities/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'Demographics/DistrictCities/EditModal');
    var dataTable = $('#DistrictCitiesTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(districtCityService.getList),
            columnDefs: [
                { title: l('Actions'), rowAction: { items: [
                    { text: l('Edit'), visible: abp.auth.isGranted('ErmanApp.DistrictCities.Edit'), action: function (data) { editModal.open({ id: data.record.id }); } },
                    { text: l('Delete'), visible: abp.auth.isGranted('ErmanApp.DistrictCities.Delete'), confirmMessage: function (data) { return l('DistrictCityDeletionConfirmationMessage', data.record.name); }, action: function (data) { districtCityService.delete(data.record.id).then(function() { abp.notify.success(l('DeletedSuccessfully')); dataTable.ajax.reload(); }); } }
                ] } },
                { title: l('Name'), data: "name" },
                { title: l('StateProvince'), data: "stateProvinceName" },
                { title: l('Population'), data: "population" },
                { title: l('CountryCode'), data: "countryCode" },
                { title: l('Latitude'), data: "latitude" },
                { title: l('Longitude'), data: "longitude" },
                { title: l('Remarks'), data: "remarks" },
                { title: l('CreationTime'), data: "creationTime", dataFormat: "datetime" }
            ]
        })
    );
    createModal.onResult(function () { abp.notify.success(l('CreatedSuccessfully')); dataTable.ajax.reload(); });
    editModal.onResult(function () { abp.notify.success(l('SavedSuccessfully')); dataTable.ajax.reload(); });
    $('#NewDistrictCityButton').click(function (e) { e.preventDefault(); createModal.open(); });
});

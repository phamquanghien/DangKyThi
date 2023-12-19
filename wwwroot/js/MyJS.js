function GetExamTimeBySubject() {
    var e = document.getElementById("SubjectID");
    var value = e.value;
    // var text = e.options[e.selectedIndex].text;
    $.ajax({
        url: "/DangKyThi/GetExamTimeBySubject",
        type: "GET",
        data: { subjectID:  value},
        success: function (response) {
            debugger;
            var html = '';
            html += '<p>Chọn Ca thi</p>'
            for (let i = 0; i < response.length; i++){
                html += '<input class="col-md-1" type="radio" name="ExamTimeID" value="' + response[i].examTimeID + '"></input>'
                html += '<label class="col-md-11" for="html">' + response[i].examTimeName + ' (Thời gian: '
                html += response[i].startTime + ' - ' + response[i].finishTime
                html += ')</label>'
            }
            $('#ExamTime').html(html)
        },
        error: function (response) {
            alert('Không kết nối được máy chủ, vui lòng thử lại sau');
        }
    });
};
// function GetSubjectGroupByStudentID(){
//     var element = document.getElementById("SubjectID");
//     var value = element.value;
//     alert(value);
// }
function GetSubjectGroupByStudentID() {
    var e = document.getElementById("SubjectID");
    var value = e.value;
    $.ajax({
        url: "/DangKyThi/GetSubjectGroupByStudentID",
        type: "GET",
        data: { subjectID:  value},
        success: function (response) {
            debugger;
            var html = '';
            html += '<label class="control-label">Chọn Nhóm môn học:</label>'
            html += '<select onchange="GetExamTimeBySubject();" class="w3-select w3-border w3-round w3-white w3-padding w3-mobile w3-col w3-large w3-margin-right" id="MySelect" name="subjectGroup"">'
            html += '<option value="-1">Chọn nhóm học phần</option>'
            for (let i = 0; i < response.length; i++){
                html += '<option value="' + response[i] + '"> Nhóm ' + response[i] +'</option>'
            }
            html += '</select>';
            $('#SubjectGroup').html(html)
        },
        error: function (response) {
            alert('Không kết nối được máy chủ, vui lòng thử lại sau');
        }
    });
};
function GetSubjectGroupBySubjectID() {
    var e = document.getElementById("SubjectID");
    var value = e.value;
    $.ajax({
        url: "/Student/GetSubjectGroupBySubjectID",
        type: "GET",
        data: { subjectID:  value},
        success: function (response) {
            debugger;
            var html = '';
            html += '<select onchange="GetExamTimeBySubject();" class="w3-select w3-border w3-round w3-white w3-padding w3-mobile w3-col w3-large w3-margin-right" id="MySelect" name="subjectGroup"">'
            html += '<option value="-1">Chọn nhóm học phần</option>'
            for (let i = 0; i < response.length; i++){
                html += '<option value="' + response[i] + '"> Nhóm ' + response[i] +'</option>'
            }
            html += '</select>';
            $('#SubjectGroup').html(html)
        },
        error: function (response) {
            alert('Không kết nối được máy chủ, vui lòng thử lại sau');
        }
    });
};
delete sajet.sys_base_param where param_name IN ('VN-VN');

Insert into SAJET.SYS_BASE_PARAM (PROGRAM, PARAM_NAME, PARAM_VALUE, PARAM_TYPE, PARAM_DESC) Values ('ALL', 'VN-VN', 'VN', 'Language', '多國語言要查的欄位
(用於SYS_PROGRAM_NAME及SYS_PROGRAM_FUN_NAME)');

Insert into SAJET.SYS_BASE_PARAM (PROGRAM, PARAM_NAME, PARAM_VALUE, PARAM_TYPE, DEFAULT_VALUE, PARAM_DESC) Values ('Query', 'VN-VN', 'REPORT_TYPE_VN,REPORT_VN', 'E', 'REPORT_TYPE_VN,REPORT_VN', 'Web Report多國語言要查的欄位');

delete sajet.sys_base where param_name in ('VN-VN');
Insert into SAJET.sys_base (PROGRAM, PARAM_NAME, PARAM_VALUE) Values ('ALL', 'VN-VN', 'VN');

COMMIT;

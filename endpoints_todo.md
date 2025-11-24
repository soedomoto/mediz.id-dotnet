# API Endpoints - MedizID Copilot

## adolescent_health.py

| Method | Endpoint | Function |
|--------|----------|----------|
| POST | `/appointments/{appointment_id}/adolescent-health` | create_adolescent_health_record |
| GET | `/appointments/{appointment_id}/adolescent-health` | get_adolescent_health_record |
| GET | `/adolescent-health/{record_id}` | get_adolescent_health_record_by_id |
| PUT | `/appointments/{appointment_id}/adolescent-health` | update_adolescent_health_record |
| PUT | `/adolescent-health/{record_id}` | update_adolescent_health_record_by_id |
| DELETE | `/appointments/{appointment_id}/adolescent-health` | delete_adolescent_health_record |
| DELETE | `/adolescent-health/{record_id}` | delete_adolescent_health_record_by_id |

## anamnesis.py

| Method | Endpoint | Function |
|--------|----------|----------|
| GET | `/` | get_anamnesis_list |
| GET | `/{anamnesis_id}` | get_anamnesis_detail |
| POST | `/` | create_anamnesis |
| PUT | `/{anamnesis_id}` | update_anamnesis |
| DELETE | `/{anamnesis_id}` | delete_anamnesis |
| GET | `/appointment/{appointment_id}` | get_anamnesis_by_appointment |
| GET | `/medical-record/{medical_record_id}` | get_anamnesis_by_medical_record |
| GET | `/templates/` | get_anamnesis_templates |
| GET | `/templates/{template_id}` | get_anamnesis_template |

## appointments.py

| Method | Endpoint | Function |
|--------|----------|----------|
| POST | `/` | create_appointment |
| GET | `/` | read_appointments |
| GET | `/{appointment_id}` | read_appointment |
| GET | `/{appointment_id}/medical-history` | get_appointment_patient_medical_history |
| PUT | `/{appointment_id}` | update_appointment |
| DELETE | `/{appointment_id}` | delete_appointment |

## auth.py

| Method | Endpoint | Function |
|--------|----------|----------|
| POST | `/google` | google_login |
| POST | `/google-old` | google_login_old |
| POST | `/login` | login_for_access_token |
| POST | `/register` | register_user |

## departments.py

| Method | Endpoint | Function |
|--------|----------|----------|
| POST | `/` | create_department |
| GET | `/` | read_departments |

## facilities/appointment.py

| Method | Endpoint | Function |
|--------|----------|----------|
| GET | `/` | get_facility_appointments |
| POST | `/` | create_facility_appointment |

## facilities/department.py

| Method | Endpoint | Function |
|--------|----------|----------|
| POST | `/` | create_department_for_facility |
| GET | `/` | list_departments |
| PUT | `/{department_id}` | update_department |

## facilities/installation.py

| Method | Endpoint | Function |
|--------|----------|----------|
| POST | `/` | create_installation |
| GET | `/` | get_facility_installations |
| GET | `/{installation_id}` | get_installation |
| PUT | `/{installation_id}` | update_installation |
| DELETE | `/{installation_id}` | delete_installation |

## facilities/patients.py

| Method | Endpoint | Function |
|--------|----------|----------|
| POST | `/` | create_facility_patient |
| GET | `/` | get_facility_patients |
| GET | `/{patient_id}/details` | get_facility_patient_details |

## facilities/polis/time_slots.py

| Method | Endpoint | Function |
|--------|----------|----------|
| POST | `/` | create_poli_time_slot |
| GET | `/` | get_poli_time_slots |
| GET | `/{slot_id}` | get_poli_time_slot |
| PUT | `/{slot_id}` | update_poli_time_slot |
| DELETE | `/{slot_id}` | delete_poli_time_slot |

## facilities/staff.py

| Method | Endpoint | Function |
|--------|----------|----------|
| POST | `/` | add_staff_to_facility |
| POST | `/` | add_staff_member |
| GET | `/` | list_staff |
| PUT | `/{staff_id}` | update_staff_member |
| DELETE | `/{staff_id}` | remove_staff_member |

## diagnosis.py

| Method | Endpoint | Function |
|--------|----------|----------|
| GET | `/appointment/{appointment_id}` | get_appointment_diagnoses |
| POST | `/` | create_appointment_diagnosis |
| PUT | `/{diagnosis_id}` | update_appointment_diagnosis |
| DELETE | `/{diagnosis_id}` | delete_appointment_diagnosis |
| GET | `/patient/{patient_id}/history` | get_patient_diagnosis_history |
| GET | `/search` | search_diagnoses |
| POST | `/ai-recommendations/{appointment_id}` | get_ai_diagnosis_recommendations |
| GET | `/ai-recommendations/{medical_record_id}` | get_ai_recommendations_history |
| PUT | `/ai-recommendations/{recommendation_id}/feedback` | update_ai_recommendation_feedback |
| GET | `/icd10` | get_icd10_codes |
| POST | `/icd10` | create_icd10_code |
| GET | `/symptoms` | get_symptoms |
| POST | `/symptoms` | create_symptom |
| GET | `/categories` | get_diagnosis_categories |
| GET | `/stats` | get_diagnosis_stats |

## laboratorium.py

| Method | Endpoint | Function |
|--------|----------|----------|
| POST | `/` | create_laboratorium_request |
| GET | `/appointments/{appointment_id}` | get_appointment_laboratorium_requests |
| GET | `/patients/{patient_id}` | get_patient_laboratorium_requests |
| GET | `/{laboratorium_id}` | get_laboratorium_request |
| PUT | `/{laboratorium_id}` | update_laboratorium_request |
| DELETE | `/{laboratorium_id}` | delete_laboratorium_request |
| GET | `/` | get_all_laboratorium_requests |

## medical_records.py

| Method | Endpoint | Function |
|--------|----------|----------|
| GET | `/` | read_medical_records |
| POST | `/` | create_medical_record |
| GET | `/{record_id}` | read_medical_record |
| PUT | `/{record_id}` | update_medical_record |
| DELETE | `/{record_id}` | delete_medical_record |

## odontogram.py

| Method | Endpoint | Function |
|--------|----------|----------|
| GET | `/options` | get_odontogram_options |
| GET | `/appointment/{appointment_id}` | get_odontogram_by_appointment |
| POST | `/` | create_odontogram |
| PUT | `/{odontogram_id}` | update_odontogram |
| GET | `/{odontogram_id}` | get_odontogram |
| DELETE | `/{odontogram_id}` | delete_odontogram |
| GET | `/` | list_odontograms |

## patients.py

| Method | Endpoint | Function |
|--------|----------|----------|
| GET | `/` | read_patients |
| POST | `/` | create_patient |
| GET | `/{patient_id}` | read_patient |
| PUT | `/{patient_id}` | update_patient |
| DELETE | `/{patient_id}` | delete_patient |

## prescriptions.py

| Method | Endpoint | Function |
|--------|----------|----------|
| GET | `/drugs/search` | search_drugs |
| GET | `/equipment/search` | search_equipment |
| GET | `/appointment/{appointment_id}` | get_appointment_prescriptions |
| POST | `/appointment/{appointment_id}` | create_appointment_prescription |
| PUT | `/appointment/{appointment_id}/{prescription_id}` | update_appointment_prescription |
| DELETE | `/appointment/{appointment_id}/{prescription_id}` | delete_appointment_prescription |
| POST | `/appointment/{appointment_id}/bulk` | create_bulk_prescriptions |
| GET | `/patient/{patient_id}/allergies` | get_patient_allergies |
| GET | `/patient/{patient_id}/history` | get_patient_prescription_history |
| POST | `/prescription/{prescription_id}/equipment` | add_prescription_equipment |
| PUT | `/prescription/{prescription_id}/dispense` | dispense_prescription |
| GET | `/stats/prescriptions` | get_prescription_statistics |
| POST | `/ai-recommendations` | get_ai_prescription_recommendations |
| GET | `/ai-recommendations/{medical_record_id}` | get_ai_prescription_history |
| PUT | `/ai-recommendations/{recommendation_id}/feedback` | update_prescription_feedback |
| POST | `/drug-interactions` | check_drug_interactions |
| GET | `/drugs` | get_drugs |
| POST | `/drugs` | create_drug |
| PUT | `/drugs/{drug_id}` | update_drug |
| GET | `/drug-categories` | get_drug_categories |
| POST | `/drug-categories` | create_drug_category |
| GET | `/equipment` | get_equipment |
| POST | `/equipment` | create_equipment |
| GET | `/equipment-types` | get_equipment_types |
| POST | `/equipment-types` | create_equipment_type |
| GET | `/stats` | get_prescription_stats |

## sti.py

| Method | Endpoint | Function |
|--------|----------|----------|
| POST | `/` | create_sti_record |
| GET | `/` | get_sti_records |
| GET | `/{sti_id}` | get_sti_record |
| GET | `/appointment/{appointment_id}` | get_sti_by_appointment |
| PUT | `/{sti_id}` | update_sti_record |
| DELETE | `/{sti_id}` | delete_sti_record |

## users.py

| Method | Endpoint | Function |
|--------|----------|----------|
| GET | `/` | read_users |
| GET | `/search/email` | search_user_by_email |
| POST | `/create-by-email` | create_user_by_email |
| GET | `/{user_id}` | read_user |
| PUT | `/{user_id}` | update_user |
| GET | `/{user_id}/appointments` | get_user_appointments |
| GET | `/{user_id}/medical-records` | get_user_medical_records |
| GET | `/{user_id}/prescriptions` | get_user_prescriptions |
| GET | `/{user_id}/facilities` | get_user_facilities |
| GET | `/{user_id}/patients` | get_user_patients |

---

**Total Endpoints: 140+**

| Module | Count |
|--------|-------|
| adolescent_health | 7 |
| anamnesis | 9 |
| appointments | 6 |
| auth | 4 |
| departments | 2 |
| diagnosis | 15 |
| facilities/appointment | 2 |
| facilities/department | 3 |
| facilities/installation | 5 |
| facilities/patients | 3 |
| facilities/polis/time_slots | 5 |
| facilities/staff | 5 |
| laboratorium | 7 |
| medical_records | 5 |
| odontogram | 7 |
| patients | 5 |
| prescriptions | 25 |
| sti | 6 |
| users | 10 |
| **TOTAL** | **142** |


using MedizID.API.Common.Enums;
using MedizID.API.Data;
using MedizID.API.Models;
using Microsoft.EntityFrameworkCore;

namespace MedizID.API.Services;

public class LaboratoriumTestMasterSeeder
{
    private readonly MedizIDDbContext _context;
    private readonly ILogger<LaboratoriumTestMasterSeeder> _logger;

    public LaboratoriumTestMasterSeeder(MedizIDDbContext context, ILogger<LaboratoriumTestMasterSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        try
        {
            // Check if data already exists
            var existingCount = await _context.LaboratoriumTestMasters.CountAsync();
            if (existingCount > 0)
            {
                _logger.LogInformation($"Laboratory test master data already exists ({existingCount} records). Skipping seed.");
                return;
            }

            _logger.LogInformation("Starting to seed laboratory test master data...");

            var testMasters = GetTestMasters();
            await _context.LaboratoriumTestMasters.AddRangeAsync(testMasters);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Successfully seeded {testMasters.Count} laboratory test master records.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error seeding laboratory test master data");
            throw;
        }
    }

    private List<LaboratoriumTestMaster> GetTestMasters()
    {
        return new List<LaboratoriumTestMaster>
        {
            // ========== DIAGNOSTIK TESTS (5 tests) ==========
            new()
            {
                Id = Guid.NewGuid(),
                TestName = "EKG",
                TestCode = "BPJS025",
                Category = LaboratoriumCategoryEnum.Diagnostik,
                Unit = "mmHg",
                ReferenceRange = "Normal sinus rhythm",
                Description = "Electrocardiogram - 12-lead ECG to assess heart rate and rhythm",
                SampleType = SampleTypeEnum.Other,
                SamplePreparation = "Patient should rest 5 minutes before test",
                IsActive = true,
                SortOrder = 1,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                TestName = "Funduskopi",
                TestCode = "BPJS027",
                Category = LaboratoriumCategoryEnum.Diagnostik,
                Unit = null,
                ReferenceRange = "Normal retinal findings",
                Description = "Fundoscopic examination to assess retinal health",
                SampleType = SampleTypeEnum.Other,
                IsActive = true,
                SortOrder = 2,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                TestName = "Echo",
                TestCode = "BPJS026",
                Category = LaboratoriumCategoryEnum.Diagnostik,
                Unit = null,
                ReferenceRange = "Normal cardiac structure and function",
                Description = "Echocardiography - ultrasound of the heart",
                SampleType = SampleTypeEnum.Other,
                IsActive = true,
                SortOrder = 3,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                TestName = "Radiologi",
                TestCode = "BPJS005",
                Category = LaboratoriumCategoryEnum.Diagnostik,
                Unit = null,
                ReferenceRange = "Normal imaging findings",
                Description = "Radiological imaging (X-ray, CT, MRI as ordered)",
                SampleType = SampleTypeEnum.Other,
                IsActive = true,
                SortOrder = 4,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                TestName = "ABI",
                TestCode = "BPJS024",
                Category = LaboratoriumCategoryEnum.Diagnostik,
                Unit = "ratio",
                ReferenceRange = ">0.90",
                Description = "Ankle-Brachial Index - assessment for peripheral arterial disease",
                SampleType = SampleTypeEnum.Other,
                IsActive = true,
                SortOrder = 5,
                CreatedAt = DateTime.UtcNow
            },

            // ========== HEMATOLOGI TESTS (9 tests) ==========
            new()
            {
                Id = Guid.NewGuid(),
                TestName = "Hemoglobin",
                TestCode = "BPJS006",
                Category = LaboratoriumCategoryEnum.Hematologi,
                Unit = "g/dL",
                ReferenceRange = "M: 13.5-17.5, W: 12-15.5",
                Description = "Blood hemoglobin level - oxygen carrying capacity",
                SampleType = SampleTypeEnum.Blood,
                SamplePreparation = "No special preparation required",
                IsActive = true,
                SortOrder = 10,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                TestName = "Leukosit",
                TestCode = "BPJS007",
                Category = LaboratoriumCategoryEnum.Hematologi,
                Unit = "x10³/μL",
                ReferenceRange = "4.5-11.0",
                Description = "White blood cell count - immune system assessment",
                SampleType = SampleTypeEnum.Blood,
                IsActive = true,
                SortOrder = 11,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                TestName = "Gula Darah Puasa",
                TestCode = "BPJS002",
                Category = LaboratoriumCategoryEnum.Hematologi,
                Unit = "mg/dL",
                ReferenceRange = "70-100",
                Description = "Fasting blood glucose - diabetes screening",
                SampleType = SampleTypeEnum.Serum,
                SamplePreparation = "Fasting 8-10 hours required",
                IsActive = true,
                SortOrder = 12,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                TestName = "Gula Darah Post Prandial",
                TestCode = "BPJS003",
                Category = LaboratoriumCategoryEnum.Hematologi,
                Unit = "mg/dL",
                ReferenceRange = "<140",
                Description = "Post-meal blood glucose - glucose tolerance",
                SampleType = SampleTypeEnum.Serum,
                SamplePreparation = "Collected 2 hours after meal",
                IsActive = true,
                SortOrder = 13,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                TestName = "Gula Darah Sewaktu",
                TestCode = "BPJS001",
                Category = LaboratoriumCategoryEnum.Hematologi,
                Unit = "mg/dL",
                ReferenceRange = "<200",
                Description = "Random blood glucose - quick glucose assessment",
                SampleType = SampleTypeEnum.Serum,
                IsActive = true,
                SortOrder = 14,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                TestName = "Trombosit",
                TestCode = "BPJS011",
                Category = LaboratoriumCategoryEnum.Hematologi,
                Unit = "x10³/μL",
                ReferenceRange = "150-400",
                Description = "Platelet count - blood clotting ability",
                SampleType = SampleTypeEnum.Blood,
                IsActive = true,
                SortOrder = 15,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                TestName = "Hematokrit",
                TestCode = "BPJS010",
                Category = LaboratoriumCategoryEnum.Hematologi,
                Unit = "%",
                ReferenceRange = "M: 41-50, W: 36-46",
                Description = "Hematocrit - percentage of red blood cells",
                SampleType = SampleTypeEnum.Blood,
                IsActive = true,
                SortOrder = 16,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                TestName = "Eritrosit",
                TestCode = "BPJS008",
                Category = LaboratoriumCategoryEnum.Hematologi,
                Unit = "x10⁶/μL",
                ReferenceRange = "M: 4.5-5.9, W: 4.1-5.1",
                Description = "Red blood cell count - oxygen transport",
                SampleType = SampleTypeEnum.Blood,
                IsActive = true,
                SortOrder = 17,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                TestName = "Laju Endap Darah",
                TestCode = "BPJS009",
                Category = LaboratoriumCategoryEnum.Hematologi,
                Unit = "mm/jam",
                ReferenceRange = "M: <15, W: <20",
                Description = "Erythrocyte Sedimentation Rate - inflammation marker",
                SampleType = SampleTypeEnum.Blood,
                IsActive = true,
                SortOrder = 18,
                CreatedAt = DateTime.UtcNow
            },

            // ========== KIMIA KLINIK TESTS (13 tests) ==========
            new()
            {
                Id = Guid.NewGuid(),
                TestName = "SGPT",
                TestCode = "BPJS017",
                Category = LaboratoriumCategoryEnum.KimiaKlinik,
                Unit = "U/L",
                ReferenceRange = "<40",
                Description = "Serum Glutamic-Pyruvic Transaminase - liver function",
                SampleType = SampleTypeEnum.Serum,
                IsActive = true,
                SortOrder = 20,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                TestName = "LDL",
                TestCode = "BPJS013",
                Category = LaboratoriumCategoryEnum.KimiaKlinik,
                Unit = "mg/dL",
                ReferenceRange = "<100",
                Description = "Low-Density Lipoprotein - bad cholesterol",
                SampleType = SampleTypeEnum.Serum,
                SamplePreparation = "Fasting 9-12 hours recommended",
                IsActive = true,
                SortOrder = 21,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                TestName = "Creatin",
                TestCode = "BPJS021",
                Category = LaboratoriumCategoryEnum.KimiaKlinik,
                Unit = "mg/dL",
                ReferenceRange = "0.6-1.2",
                Description = "Creatinine - kidney function marker",
                SampleType = SampleTypeEnum.Serum,
                IsActive = true,
                SortOrder = 22,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                TestName = "SGOT",
                TestCode = "BPJS016",
                Category = LaboratoriumCategoryEnum.KimiaKlinik,
                Unit = "U/L",
                ReferenceRange = "<40",
                Description = "Serum Glutamic-Oxaloacetic Transaminase - liver/heart assessment",
                SampleType = SampleTypeEnum.Serum,
                IsActive = true,
                SortOrder = 23,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                TestName = "HbA1c",
                TestCode = "BPJS004",
                Category = LaboratoriumCategoryEnum.KimiaKlinik,
                Unit = "%",
                ReferenceRange = "<5.7",
                Description = "Hemoglobin A1c - long-term glucose control (3 months)",
                SampleType = SampleTypeEnum.Blood,
                IsActive = true,
                SortOrder = 24,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                TestName = "Ureaum",
                TestCode = "BPJS022",
                Category = LaboratoriumCategoryEnum.KimiaKlinik,
                Unit = "mg/dL",
                ReferenceRange = "7-20",
                Description = "Blood Urea Nitrogen - kidney function",
                SampleType = SampleTypeEnum.Serum,
                IsActive = true,
                SortOrder = 25,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                TestName = "HDL",
                TestCode = "BPJS012",
                Category = LaboratoriumCategoryEnum.KimiaKlinik,
                Unit = "mg/dL",
                ReferenceRange = ">40 (M), >50 (W)",
                Description = "High-Density Lipoprotein - good cholesterol",
                SampleType = SampleTypeEnum.Serum,
                SamplePreparation = "Fasting 9-12 hours recommended",
                IsActive = true,
                SortOrder = 26,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                TestName = "Protein Kualitatif Urin",
                TestCode = "BPJS019",
                Category = LaboratoriumCategoryEnum.KimiaKlinik,
                Unit = "mg/24jam",
                ReferenceRange = "<150",
                Description = "Urine protein - kidney function assessment",
                SampleType = SampleTypeEnum.Urine,
                SamplePreparation = "24-hour urine collection",
                IsActive = true,
                SortOrder = 27,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                TestName = "Cholesterol Total",
                TestCode = "BPJS014",
                Category = LaboratoriumCategoryEnum.KimiaKlinik,
                Unit = "mg/dL",
                ReferenceRange = "<200",
                Description = "Total serum cholesterol - cardiovascular risk",
                SampleType = SampleTypeEnum.Serum,
                SamplePreparation = "Fasting 9-12 hours recommended",
                IsActive = true,
                SortOrder = 28,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                TestName = "Trigliserid",
                TestCode = "BPJS015",
                Category = LaboratoriumCategoryEnum.KimiaKlinik,
                Unit = "mg/dL",
                ReferenceRange = "<150",
                Description = "Triglycerides - lipid metabolism",
                SampleType = SampleTypeEnum.Serum,
                SamplePreparation = "Fasting 9-12 hours required",
                IsActive = true,
                SortOrder = 29,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                TestName = "Gamma GT",
                TestCode = "BPJS018",
                Category = LaboratoriumCategoryEnum.KimiaKlinik,
                Unit = "U/L",
                ReferenceRange = "M: <55, W: <38",
                Description = "Gamma-Glutamyl Transferase - liver function",
                SampleType = SampleTypeEnum.Serum,
                IsActive = true,
                SortOrder = 30,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                TestName = "Albumin",
                TestCode = "BPJS020",
                Category = LaboratoriumCategoryEnum.KimiaKlinik,
                Unit = "g/dL",
                ReferenceRange = "3.4-5.4",
                Description = "Serum albumin - protein status and liver function",
                SampleType = SampleTypeEnum.Serum,
                IsActive = true,
                SortOrder = 31,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                TestName = "Asam Urat",
                TestCode = "BPJS023",
                Category = LaboratoriumCategoryEnum.KimiaKlinik,
                Unit = "mg/dL",
                ReferenceRange = "M: 3.5-7.2, W: 2.6-6.0",
                Description = "Uric acid - gout and kidney disease screening",
                SampleType = SampleTypeEnum.Serum,
                IsActive = true,
                SortOrder = 32,
                CreatedAt = DateTime.UtcNow
            },

            // ========== LAIN-LAIN (1 test) ==========
            new()
            {
                Id = Guid.NewGuid(),
                TestName = "Pemeriksaan Lainnya",
                TestCode = "BPJS028",
                Category = LaboratoriumCategoryEnum.LainLain,
                Unit = null,
                ReferenceRange = "Varies",
                Description = "Other laboratory tests as ordered by physician",
                SampleType = null,
                IsActive = true,
                SortOrder = 50,
                CreatedAt = DateTime.UtcNow
            }
        };
    }
}

using IdentityModel;
using IdentityService;
using MedicalModel;
using MedicalService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;

namespace Asm2
{
    public static class MedicalDbInitialize
    {
        public static void EnsureMedicalDbCreated(this IApplicationBuilder appBuilder)
        {
            using (var scope = appBuilder.ApplicationServices.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<MedicalDbContext>();
                db.Database.EnsureCreated();
            }
        }

        public static async Task SeedMedicalData(this IApplicationBuilder builder)
        {
            using (var scope = builder.ApplicationServices.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<MedicalDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!db.Hospitals.Any())
                {
                    // Add Hanoi hospitals
                    db.Hospitals.AddRange(
                        new Hospital { Name = "Bệnh Viện Bạch Mai", PhoneNumber = "0969851616", Address = "78 Đường Giải Phóng, Phương Mai, Quận Đống Đa", Region = "Hanoi", Country = "VN" },
                        new Hospital { Name = "Bệnh Viện Bệnh Nhiệt Đới Trung Ương", PhoneNumber = "0969241616", Address = "78 Giải Phóng, Phương Mai, Quận Đống Đa",  Region = "Hanoi", Country = "VN" },
                        new Hospital { Name = "Bệnh Viện Châm Cứu Trung Ương", PhoneNumber = "0969231616", Address = "49 Thái Thịnh, Quận Đống Đa", Region = "Hanoi", Country = "VN" },
                        new Hospital { Name = "Bệnh Viện Da Liễu Trung Ương", PhoneNumber = "0969201616", Address = "15A Phương Mai, Quận Đống Đa", Region = "Hanoi", Country = "VN" },
                        new Hospital { Name = "Bệnh Viện Hữu Nghị Việt Đức", PhoneNumber = "0967991616", Address = "40 Tràng Thi, Quận Hoàn Kiếm", Region = "Hanoi", Country = "VN" },
                        new Hospital { Name = "Bệnh Viện K", PhoneNumber = "0967721616", Address = "43 Quán Sứ, Quận Hoàn Kiếm", Region = "Hanoi", Country = "VN" },
                        new Hospital { Name = "Bệnh Viện Lão Khoa Trung Ương", PhoneNumber = "0967971616", Address = "1A Phương Mai, Quận Đống Đa", Region = "Hanoi", Country = "VN" },
                        new Hospital { Name = "Bệnh Viện Mắt Trung Ương", PhoneNumber = "09670961616", Address = "85 Bà Triệu, Phường Nguyễn Du, Quận Hai Bà Trưng", Region = "Hanoi", Country = "VN" },
                        new Hospital { Name = "Bệnh Viện Nhi Trung Ương", PhoneNumber = "0967951616", Address = "Số 18, Ngõ 879, La Thành, Quận Đống Đa", Region = "Hanoi", Country = "VN" },
                        new Hospital { Name = "Bệnh Viện Phổi Trung Ương", PhoneNumber = "0967941616", Address = "463 Hoàng Hoa Thám, Quận Ba Đình", Region = "Hanoi", Country = "VN" },
                        new Hospital { Name = "Bệnh Viện Phụ Sản Trung Ương", PhoneNumber = "0967931616", Address = "43 Tràng Thi, Hàng Bông, Quận Hoàn Kiếm", Region = "Hanoi", Country = "VN" },
                        new Hospital { Name = "Bệnh Viện Răng Hàm Mặt Trung Ương", PhoneNumber = "0967921616", Address = "40 Tràng Thi, Hàng Bông, Quận Hoàn Kiếm", Region = "Hanoi", Country = "VN" },
                        new Hospital { Name = "Bệnh Viện Tai Mũi Họng Trung Ương", PhoneNumber = "0967911616", Address = "78 Giải Phóng, Quận Đống Đa", Region = "Hanoi", Country = "VN" },
                        new Hospital { Name = "Bệnh Viện Tâm Thần Trung Ương 1", PhoneNumber = "0967901616", Address = "Hoà Bình, Huyện Thường Tín", Region = "Hanoi", Country = "VN" },
                        new Hospital { Name = "Viện Huyết Học - Truyền Máu Trung Ương", PhoneNumber = "0967891616", Address = "Yên Hòa, Quận Cầu Giấy", Region = "Hanoi", Country = "VN" },
                        new Hospital { Name = "Viện Vệ sinh dịch tễ Trung Ương", PhoneNumber = "0967881616", Address = "Số 1 Yessin, Quận Hai Bà Trưng", Region = "Hanoi", Country = "VN" },
                        new Hospital { Name = "Bệnh Viện E", PhoneNumber = "0967871616", Address = "91 Trần Cung, Quận Cầu Giấy", Region = "Hanoi", Country = "VN" },
                        new Hospital { Name = "Bệnh Viện Hữu Nghị", PhoneNumber = "0967861616", Address = "1 Trần Khánh Dư, Quận Hai Bà Trưng", Region = "Hanoi", Country = "VN" },
                        new Hospital { Name = "Bệnh Viện Lao và Bệnh Phổi Trung Ương", PhoneNumber = "0967851616", Address = "463 Hoàng Hoa Thám, Quận Ba Đình", Region = "Hanoi", Country = "VN" },
                        new Hospital { Name = "Bệnh Viện Nội Tiết Trung Ương", PhoneNumber = "0967841616", Address = "Khu B Yên Lãng, Thịnh Quang, Quận Đống Đa", Region = "Hanoi", Country = "VN" },
                        new Hospital { Name = "Bệnh Viện Y Học Cổ Truyền Trung Ương", PhoneNumber = "0967821616", Address = "29 Nguyễn Bỉnh Khiêm, Quận Hai Bà Trưng", Region = "Hanoi", Country = "VN" },
                        new Hospital { Name = "Bệnh Viện 103", PhoneNumber = "0967811616", Address = "Km 3 Đường 70, Tân Triều, Huyện Thanh Trì", Region = "Hanoi", Country = "VN" },
                        new Hospital { Name = "Bệnh Viện 198", PhoneNumber = "0967801616", Address = "Số 9 Trần Bình, Mai Dịch, Quận Cầu Giấy", Region = "Hanoi", Country = "VN" },
                        new Hospital { Name = "Bệnh Viện Bưu Điện", PhoneNumber = "0967791616", Address = "49 Trần Điền, Phường Định Công, Quận Hoàng Mai", Region = "Hanoi", Country = "VN" },
                        new Hospital { Name = "Bệnh Viện Nông Nghiệp", PhoneNumber = "0967781616", Address = "Ngọc Hồi, Huyện Thanh Trì", Region = "Hanoi", Country = "VN" },
                        new Hospital { Name = "Bệnh Viện Y Học Cổ Truyền TP Hà Nội", PhoneNumber = "0967771616", Address = "278 Lương Thế Vinh, Quận Thanh Xuân", Region = "Hanoi", Country = "VN" },
                        new Hospital { Name = "Viện Bỏng Quốc gia", PhoneNumber = "0967761616", Address = "Tân Triều, Huyện Thanh Trì", Region = "Hanoi", Country = "VN" },
                        new Hospital { Name = "Bệnh viện Quân Y 108", PhoneNumber = "0967751616", Address = "Số 1 Trần Hưng Đạo, Quận Hoàn Kiếm", Region = "Hanoi", Country = "VN" },
                        new Hospital { Name = "Bệnh viện Xây dựng", PhoneNumber = "0967741616", Address = "Nguyễn Quí Đức, Quận Thanh Xuân", Region = "Hanoi", Country = "VN" },
                        new Hospital { Name = "Bệnh viện Giao Thông Vận Tải", PhoneNumber = "0967731616", Address = "Ngõ 1194 Đường Láng, Quận Đống Đa", Region = "Hanoi", Country = "VN" },
                        new Hospital { Name = "Bệnh Viện Đa Khoa Xanh Pôn", PhoneNumber = "0967711616", Address = "12 Chu Văn An, Quận Ba Đình", Region = "Hanoi", Country = "VN" },
                        new Hospital { Name = "Bệnh Viện Y Học Cổ Truyền Hà Nội", PhoneNumber = "0967701616", Address = "8 Phạm Hùng, Mai Dịch, Quận Cầu Giấy", Region = "Hanoi", Country = "VN" },
                        new Hospital { Name = "Bệnh Viện Da Liễu Hà Nội", PhoneNumber = "0967691616", Address = "79B Nguyễn Khuyến, Văn Miếu, Quận Đống Đa", Region = "Hanoi", Country = "VN" },
                        new Hospital { Name = "Bệnh Viện Điều Dưỡng & PHCN Hà Nội", PhoneNumber = "0967681616", Address = "35 Lê Văn Thiêm, Thanh Xuân Trung, Quận Thanh Xuân", Region = "Hanoi", Country = "VN" },
                        new Hospital { Name = "Bệnh Viện Hữu Nghị Việt Nam Cuba", PhoneNumber = "0967671616", Address = "37 Hai Bà Trưng, Tràng Tiền, Quận Hoàn Kiếm", Region = "Hanoi", Country = "VN" },
                        new Hospital { Name = "Bệnh Viện Mắt Hà Đông", PhoneNumber = "0967341616", Address = "2D Nguyễn Viết Xuân, Quận Hà Đông", Region = "Hanoi", Country = "VN" },
                        new Hospital { Name = "Bệnh Viện Mắt Hà Nội", PhoneNumber = "0967331616", Address = "37 Hai Bà Trưng, Tràng Tiền, Quận Hoàn Kiếm", Region = "Hanoi", Country = "VN" },
                        new Hospital { Name = "Bệnh Viện Nam Thăng Long", PhoneNumber = "0967321616", Address = "38 Tân Xuân, Xuân Đỉnh, Huyện Từ Liêm", Region = "Hanoi", Country = "VN" },
                        new Hospital { Name = "Bệnh Viện Phụ Sản Hà Nội", PhoneNumber = "0967311616", Address = "La Thành, Ngọc Khánh, Quận Ba Đình", Region = "Hanoi", Country = "VN" },
                        new Hospital { Name = "Bệnh Viện Tâm Thần Hà Nội", PhoneNumber = "0967301616", Address = "Ngõ 467 Nguyễn Văn Linh, Sài Đồng, Quận Long Biên", Region = "Hanoi", Country = "VN" },
                        new Hospital { Name = "Bệnh Viện Tâm Thần Mỹ Đức", PhoneNumber = "0967291616", Address = "Phúc Lâm, Huyện Mỹ Đức", Region = "Hanoi", Country = "VN" },
                        new Hospital { Name = "Bệnh Viện Thận Hà Nội", PhoneNumber = "0966891616", Address = "70 Nguyễn Chí Thanh" });
                    // add HCM city hospitals
                    db.Hospitals.AddRange(
                        new Hospital { Name = "Bệnh viện Nhân dân 115", PhoneNumber = "0968001115", Address = "Thành phố Hồ Chí Minh", Region = "HCM City", Country = "VN" },
                        new Hospital { Name = "Bệnh viện Nhân dân Gia Định", PhoneNumber = "0968001122", Address = "Thành phố Hồ Chí Minh", Region = "HCM City", Country = "VN" },
                        new Hospital { Name = "Bệnh viện Nguyễn Trãi", PhoneNumber = "0968001133", Address = "Thành phố Hồ Chí Minh", Region = "HCM City", Country = "VN" },
                        new Hospital { Name = "Bệnh viện Nguyễn Tri Phương", PhoneNumber = "0968001144", Address = "Thành phố Hồ Chí Minh", Region = "HCM City", Country = "VN" },
                        new Hospital { Name = "Bệnh viện Cấp cứu Trưng Vương", PhoneNumber = "0968001155", Address = "Thành phố Hồ Chí Minh", Region = "HCM City", Country = "VN" },
                        new Hospital { Name = "Bệnh viện Tai Mũi Họng", PhoneNumber = "0968001166", Address = "Thành phố Hồ Chí Minh", Region = "HCM City", Country = "VN" },
                        new Hospital { Name = "Bệnh viện Mắt", PhoneNumber = "0968001177", Address = "Thành phố Hồ Chí Minh", Region = "HCM City", Country = "VN" },
                        new Hospital { Name = "Bệnh viện Chấn thương chỉnh hình", PhoneNumber = "0968001188", Address = "Thành phố Hồ Chí Minh", Region = "HCM City", Country = "VN" },
                        new Hospital { Name = "Bệnh viện Bình Dân", PhoneNumber = "0968001199", Address = "Thành phố Hồ Chí Minh", Region = "HCM City", Country = "VN" },
                        new Hospital { Name = "Bệnh viện Răng Hàm Mặt", PhoneNumber = "0968001200", Address = "Thành phố Hồ Chí Minh", Region = "HCM City", Country = "VN" },
                        new Hospital { Name = "Bệnh viện Bệnh Nhiệt đới", PhoneNumber = "0968001211", Address = "Thành phố Hồ Chí Minh", Region = "HCM City", Country = "VN" },
                        new Hospital { Name = "Bệnh viện Từ Dũ", PhoneNumber = "0968001222", Address = "Thành phố Hồ Chí Minh", Region = "HCM City", Country = "VN" },
                        new Hospital { Name = "Bệnh viện Hùng Vương", PhoneNumber = "0968001233", Address = "Thành phố Hồ Chí Minh", Region = "HCM City", Country = "VN" },
                        new Hospital { Name = "Bệnh viện Nhi đồng 1", PhoneNumber = "0968001244", Address = "Thành phố Hồ Chí Minh", Region = "HCM City", Country = "VN" },
                        new Hospital { Name = "Bệnh viện Nhi đồng 2", PhoneNumber = "0968001255", Address = "Thành phố Hồ Chí Minh", Region = "HCM City", Country = "VN" },
                        new Hospital { Name = "Bệnh viện Phạm Ngọc Thạch", PhoneNumber = "0968001266", Address = "Thành phố Hồ Chí Minh", Region = "HCM City", Country = "VN" },
                        new Hospital { Name = "Bệnh viện Ung Bướu", PhoneNumber = "0968001277", Address = "Thành phố Hồ Chí Minh", Region = "HCM City", Country = "VN" },
                        new Hospital { Name = "Viện Tim Thành phố Hồ Chí Minh", PhoneNumber = "0968001288", Address = "Thành phố Hồ Chí Minh", Region = "HCM City", Country = "VN" },
                        new Hospital { Name = "Viện Y Dược học cổ truyền", PhoneNumber = "0968001299", Address = "Thành phố Hồ Chí Minh", Region = "HCM City", Country = "VN" },
                        new Hospital { Name = "Bệnh viện Da Liễu", PhoneNumber = "0968001300", Address = "Thành phố Hồ Chí Minh", Region = "HCM City", Country = "VN" },
                        new Hospital { Name = "Bệnh viện Truyền máu Huyết học", PhoneNumber = "0968001311", Address = "Thành phố Hồ Chí Minh", Region = "HCM City", Country = "VN" },
                        new Hospital { Name = "Bệnh viện Nhi đồng Thành phố", PhoneNumber = "0968001322", Address = "Thành phố Hồ Chí Minh", Region = "HCM City", Country = "VN" },
                        new Hospital { Name = "Bệnh viện An Bình", PhoneNumber = "0968001333", Address = "Thành phố Hồ Chí Minh", Region = "HCM City", Country = "VN" },
                        new Hospital { Name = "Bệnh viện Nhân Ái", PhoneNumber = "0968001344", Address = "Thành phố Hồ Chí Minh", Region = "HCM City", Country = "VN" },
                        new Hospital { Name = "Bệnh viện Tâm Thần", PhoneNumber = "0968001355", Address = "Thành phố Hồ Chí Minh", Region = "HCM City", Country = "VN" },
                        new Hospital { Name = "Bệnh viện Lao và Bệnh Phổi", PhoneNumber = "0968001366", Address = "Thành phố Hồ Chí Minh" });
                    // ... more entries continue (including district hospitals and medical centers)

                    db.SaveChanges();
                }
                if (!db.Persons.Any())
                {
                    var saintPaulHospital = db.Hospitals.FirstOrDefault(h => h.Name == "Bệnh Viện Đa Khoa Xanh Pôn");

                    db.Persons.AddRange(
                        new Person { FirstName = "Đức Long", LastName = "Nguyễn", Gender = Gender.Male, Roles = new() { PersonRole.Doctor, PersonRole.Patient }, DoctorInfo = new DoctorInfo { HospitalId = saintPaulHospital.Id, Specialty = "", PositionAtHospital = "Giám đốc bệnh viện" }, Email = "doremon1901@gmail.com" },
                        new Person { FirstName = "Ngọc Sơn", LastName = "Trần", Gender = Gender.Male, Roles = new() { PersonRole.Doctor, PersonRole.Patient }, DoctorInfo = new DoctorInfo { HospitalId = saintPaulHospital.Id, Specialty = "Phẫu thuật nhi", PositionAtHospital = "Phó giám đốc bệnh viện - Trưởng khoa Phẫu thuật nhi" }, Email = "doremon1902@gmail.com" },
                        new Person { FirstName = "Văn Quyết", LastName = "Trần", Gender = Gender.Male, Roles = new() { PersonRole.Doctor, PersonRole.Patient }, DoctorInfo = new DoctorInfo { HospitalId = saintPaulHospital.Id, Specialty = "Phẫu thuật nhi", PositionAtHospital = "Phó khoa Phẫu thuật nhi" }, Email = "doremon1903@gmail.com" },
                        new Person { FirstName = "Liên Hương", LastName = "Trần", Gender = Gender.Female, Roles = new() { PersonRole.Doctor, PersonRole.Patient }, DoctorInfo = new DoctorInfo { HospitalId = saintPaulHospital.Id, Specialty = "", PositionAtHospital = "Phó giám đốc bệnh viện" }, Email = "doremon1904@gmail.com" },
                        new Person { FirstName = "Văn Trung", LastName = "Trần", Gender = Gender.Male, Roles = new() { PersonRole.Doctor, PersonRole.Patient }, DoctorInfo = new DoctorInfo { HospitalId = saintPaulHospital.Id, Specialty = "Hồi sức cấp cứu nhi", PositionAtHospital = "Trưởng khoa Hồi sức cấp cứu nhi" }, Email = "doremon1905@gmail.com" },
                        new Person { FirstName = "Trung Hiếu", LastName = "Nguyễn", Gender = Gender.Male, Roles = new() { PersonRole.Doctor, PersonRole.Patient }, DoctorInfo = new DoctorInfo { HospitalId = saintPaulHospital.Id, Specialty = "Hồi sức cấp cứu nhi", PositionAtHospital = "Phó trưởng khoa Hồi sức cấp cứu nhi" }, Email = "doremon1906@gmail.com" },
                        new Person { FirstName = "Thu Phương", LastName = "Lương", Gender = Gender.Female, Roles = new() { PersonRole.Doctor, PersonRole.Patient }, DoctorInfo = new DoctorInfo { HospitalId = saintPaulHospital.Id, Specialty = "Tim mạch nhi", PositionAtHospital = "Trưởng khoa Tim mạch nhi" }, Email = "doremon1907@gmail.com" },
                        new Person { FirstName = "Quế Phương", LastName = "Nguyễn", Gender = Gender.Female, Roles = new() { PersonRole.Doctor, PersonRole.Patient }, DoctorInfo = new DoctorInfo { HospitalId = saintPaulHospital.Id, Specialty = "Tim mạch nhi", PositionAtHospital = "Phó trưởng khoa Tim mạch nhi" }, Email = "doremon1908@gmail.com" },
                        new Person { FirstName = "Mỹ Hòa", LastName = "Võ Thị", Gender = Gender.Female, Roles = new() { PersonRole.Doctor, PersonRole.Patient }, DoctorInfo = new DoctorInfo { HospitalId = saintPaulHospital.Id, Specialty = "Hô hấp nhi", PositionAtHospital = "Trưởng khoa Hô hấp nhi" }, Email = "doremon1909@gmail.com" },
                        new Person { FirstName = "Kim Dung", LastName = "Phan Thị", Gender = Gender.Female, Roles = new() { PersonRole.Doctor, PersonRole.Patient }, DoctorInfo = new DoctorInfo { HospitalId = saintPaulHospital.Id, Specialty = "Hô hấp nhi", PositionAtHospital = "Phó trưởng khoa Hô hấp nhi" }, Email = "doremon1910@gmail.com" }
                    );
                    // ... more entries continue 
                    db.SaveChanges();

                    // add identity for these doctors
                    var persons = db.Persons.ToList();
                    foreach (var person in persons)
                    {
                        var user = new ApplicationUser
                        {
                            UserName = person.Email,
                            Email = person.Email,
                            Id = person.Id                            
                        };

                        userManager.CreateAsync(user, "*Abc01234").GetAwaiter().GetResult();
                        // assign roles to the user
                        userManager.AddToRoleAsync(user, UserRoles.USER).GetAwaiter().GetResult();

                    }
                }
            }
        }
    }
}

// ITLegend Admin Dashboard Script
// Complete functionality for managing diplomas, packages, courses, and reviews

// Translation System
const translations = {
  en: {
    searchPlaceholder: "Search courses, students, or anything...",
    navMain: "Main",
    navDashboard: "Dashboard",
    navCourses: "Courses",
    navDiplomas: "Diplomas",
    navPackages: "Packages",
    navCourseManagement: "Course Management",
    breadcrumbHome: "Home",
    breadcrumbDashboard: "Dashboard",
    breadcrumbDiplomas: "Diplomas",
    dashboardTitle: "Dashboard",
    dashboardDescription: "Welcome back, Admin! Here's what's happening with your platform today.",
    btnExportReport: "Export Report",
    btnAddNew: "Add New",
    btnFilter: "Filter",
    btnView: "View",
    btnEdit: "Edit",
    btnDelete: "Delete",
    btnSave: "Save",
    btnCancel: "Cancel",
    metricTotalCourses: "Total Courses",
    metricCoursesChange: "12% from last month",
    metricTotalStudents: "Total Students",
    metricStudentsChange: "8% from last month",
    metricRevenue: "Revenue",
    metricRevenueChange: "18% from last month",
    metricCompletionRate: "Completion Rate",
    metricCompletionChange: "3% from last month",
    metricTotalDiplomas: "Total Diplomas",
    metricDiplomasChange: "24% from last month",
    cardCourseEnrollment: "Course Enrollment",
    cardRecentActivity: "Recent Activity",
    cardAllDiplomas: "All Diplomas",
    diplomasDescription: "Manage and view all diplomas in the system",
    packagesDescription: "Manage and view all packages and their courses",
    loginTitle: "Login",
    loginDescription: "Welcome to ITLegend Educational Platform",
    emailLabel: "Email",
    emailPlaceholder: "Enter your email",
    passwordLabel: "Password",
    passwordPlaceholder: "Enter your password",
    rememberMe: "Remember me",
    forgotPassword: "Forgot password?",
    loginButton: "Login",
    logout: "Logout",
    paginationInfo: "Showing 1 to 6 of 856 entries",
  },
  ar: {
    searchPlaceholder: "ابحث عن الدورات أو الطلاب أو أي شيء...",
    navMain: "الرئيسية",
    navDashboard: "لوحة التحكم",
    navCourses: "الدورات",
    navDiplomas: "الدبلومات",
    navPackages: "الباقات",
    navCourseManagement: "إدارة الدورات",
    breadcrumbHome: "الرئيسية",
    breadcrumbDashboard: "لوحة التحكم",
    breadcrumbDiplomas: "الدبلومات",
    dashboardTitle: "لوحة التحكم",
    dashboardDescription: "مرحباً بعودتك، المسؤول! إليك ما يحدث في منصتك اليوم.",
    btnExportReport: "تصدير التقرير",
    btnAddNew: "إضافة جديد",
    btnFilter: "تصفية",
    btnView: "عرض",
    btnEdit: "تعديل",
    btnDelete: "حذف",
    btnSave: "حفظ",
    btnCancel: "إلغاء",
    metricTotalCourses: "إجمالي الدورات",
    metricCoursesChange: "12% من الشهر الماضي",
    metricTotalStudents: "إجمالي الطلاب",
    metricStudentsChange: "8% من الشهر الماضي",
    metricRevenue: "الإيرادات",
    metricRevenueChange: "18% من الشهر الماضي",
    metricCompletionRate: "معدل الإكمال",
    metricCompletionChange: "3% من الشهر الماضي",
    metricTotalDiplomas: "إجمالي الدبلومات",
    metricDiplomasChange: "24% من الشهر الماضي",
    cardCourseEnrollment: "التسجيل في الدورات",
    cardRecentActivity: "النشاط الأخير",
    cardAllDiplomas: "جميع الدبلومات",
    diplomasDescription: "إدارة وعرض جميع الدبلومات في النظام",
    packagesDescription: "إدارة وعرض جميع الباقات والدورات المرتبطة بها",
    loginTitle: "تسجيل الدخول",
    loginDescription: "مرحباً بك في منصة ITLegend التعليمية",
    emailLabel: "البريد الإلكتروني",
    emailPlaceholder: "أدخل بريدك الإلكتروني",
    passwordLabel: "كلمة المرور",
    passwordPlaceholder: "أدخل كلمة المرور",
    rememberMe: "تذكرني",
    forgotPassword: "نسيت كلمة المرور؟",
    loginButton: "تسجيل الدخول",
    logout: "تسجيل الخروج",
    paginationInfo: "عرض 1 إلى 6 من 856 إدخال",
  },
};

// Global Variables
let currentLanguage = "ar";
let currentDiplomaId = null;
let currentPackageId = null;
let currentDiplomasView = "grid";
let currentPackagesView = "grid";
let currentCoursesView = "grid";

// DOM Elements
const menuToggle = document.getElementById("menuToggle");
const sidebar = document.getElementById("sidebar");
const overlay = document.getElementById("overlay");
const mainContent = document.getElementById("mainContent");
const themeToggle = document.getElementById("themeToggle");
const languageToggle = document.getElementById("languageToggle");
const pageLinks = document.querySelectorAll("[data-page]");
const pages = document.querySelectorAll(".page");
const navItems = document.querySelectorAll(".nav-item");

const notificationBtn = document.getElementById("notificationBtn");
const notificationMenu = document.getElementById("notificationMenu");
const userBtn = document.getElementById("userBtn");
const userMenu = document.getElementById("userMenu");

const loginPage = document.getElementById("login-page");
const loginForm = document.getElementById("loginForm");
const loginLanguageToggle = document.getElementById("loginLanguageToggle");
const loginThemeToggle = document.getElementById("loginThemeToggle");

const logoutBtn = document.getElementById("logoutBtn");

// Data Management Functions
function initializeSampleData() {
    localStorage.clear();
  if (!localStorage.getItem("diplomas")) {
    const sampleDiplomas = [
      {
        id: "1",
        titleAr: "دبلوم تطوير الويب الكامل",
        titleEn: "Full Stack Web Development Diploma",
        categoryCode: 101,
        shortDescriptionAr: "تعلم تطوير الويب من الصفر إلى الاحتراف",
        shortDescriptionEn: "Learn web development from scratch to professional",
        featureTitleAr: "تطوير تطبيقات ويب متكاملة",
        featureTitleEn: "Develop complete web applications",
        featureShortDescriptionAr: "تعلم بناء تطبيقات ويب حديثة باستخدام أحدث التقنيات",
        featureShortDescriptionEn: "Learn to build modern web applications using the latest technologies",
        image: "https://picsum.photos/seed/webdev/800/600.jpg",
        videoPath: "https://www.youtube.com/watch?v=example",
        displayOrder: 1,
        priority: 1,
        isBestSeller: true,
        categoryTypeId: 1,
        currentState: 1,
        createdDate: new Date().toISOString(),
      },
      {
        id: "2",
        titleAr: "دبلوم تطوير تطبيقات الموبايل",
        titleEn: "Mobile App Development Diploma",
        categoryCode: 102,
        shortDescriptionAr: "تطوير تطبيقات iOS و Android",
        shortDescriptionEn: "Develop iOS and Android applications",
        featureTitleAr: "تطوير تطبيقات موبايل احترافية",
        featureTitleEn: "Develop professional mobile applications",
        featureShortDescriptionAr: "تعلم بناء تطبيقات موبايل لنظامي iOS و Android",
        featureShortDescriptionEn: "Learn to build mobile apps for iOS and Android",
        image: "https://picsum.photos/seed/mobiledev/800/600.jpg",
        videoPath: "",
        displayOrder: 2,
        priority: 2,
        isBestSeller: false,
        categoryTypeId: 1,
        currentState: 1,
        createdDate: new Date().toISOString(),
      },
      {
        id: "3",
        titleAr: "دبلوم علوم البيانات",
        titleEn: "Data Science Diploma",
        categoryCode: 103,
        shortDescriptionAr: "تحليل البيانات والذكاء الاصطناعي",
        shortDescriptionEn: "Data analysis and artificial intelligence",
        featureTitleAr: "تحليل البيانات والتعلم الآلي",
        featureTitleEn: "Data analysis and machine learning",
        featureShortDescriptionAr: "تعلم تحليل البيانات وبناء نماذج الذكاء الاصطناعي",
        featureShortDescriptionEn: "Learn data analysis and build AI models",
        image: "https://picsum.photos/seed/datascience/800/600.jpg",
        videoPath: "",
        displayOrder: 3,
        priority: 3,
        isBestSeller: true,
        categoryTypeId: 1,
        currentState: 1,
        createdDate: new Date().toISOString(),
      },
    ];
    localStorage.setItem("diplomas", JSON.stringify(sampleDiplomas));
  }

  if (!localStorage.getItem("packages")) {
    const samplePackages = [
      {
        id: "1",
        titleAr: "الباقة الأساسية",
        titleEn: "Basic Package",
        shortDescriptionAr: "الباقة الأساسية للمبتدئين",
        shortDescriptionEn: "Basic package for beginners",
        categoryId: "1",
        rank: 1,
        generalDiscount: 0,
        upgradeDiscount: 0,
        defaultDiscountRate: 10,
        isDefault: true,
        isLastPackage: false,
        currentState: 1,
        createdDate: new Date().toISOString(),
      },
      {
        id: "2",
        titleAr: "الباقة المتقدمة",
        titleEn: "Advanced Package",
        shortDescriptionAr: "الباقة المتقدمة للمحترفين",
        shortDescriptionEn: "Advanced package for professionals",
        categoryId: "1",
        rank: 2,
        generalDiscount: 15,
        upgradeDiscount: 10,
        defaultDiscountRate: 20,
        isDefault: false,
        isLastPackage: false,
        currentState: 1,
        createdDate: new Date().toISOString(),
      },
      {
        id: "3",
        titleAr: "الباقة الشاملة",
        titleEn: "Complete Package",
        shortDescriptionAr: "الباقة الشاملة مع جميع الدورات",
        shortDescriptionEn: "Complete package with all courses",
        categoryId: "1",
        rank: 3,
        generalDiscount: 25,
        upgradeDiscount: 20,
        defaultDiscountRate: 30,
        isDefault: false,
        isLastPackage: true,
        currentState: 1,
        createdDate: new Date().toISOString(),
      },
    ];
    localStorage.setItem("packages", JSON.stringify(samplePackages));
  }

  if (!localStorage.getItem("categoryFeatures")) {
    const sampleFeatures = [
      {
        id: "1",
        categoryId: "1",
        titleAr: "مشاريع عملية",
        titleEn: "Practical Projects",
        descriptionAr: "بناء مشاريع حقيقية خلال الدبلوم",
        descriptionEn: "Build real projects during the diploma",
        icon: "fa-project-diagram",
        displayOrder: 1,
      },
      {
        id: "2",
        categoryId: "1",
        titleAr: "دعم فني مستمر",
        titleEn: "Continuous Technical Support",
        descriptionAr: "دعم فني على مدار الساعة",
        descriptionEn: "24/7 technical support",
        icon: "fa-headset",
        displayOrder: 2,
      },
    ];
    localStorage.setItem("categoryFeatures", JSON.stringify(sampleFeatures));
  }

  if (!localStorage.getItem("categoryJobs")) {
    const sampleJobs = [
      {
        id: "1",
        categoryId: "1",
        titleAr: "مطور ويب Full Stack",
        titleEn: "Full Stack Web Developer",
        descriptionAr: "تطوير تطبيقات ويب متكاملة",
        descriptionEn: "Develop complete web applications",
        displayOrder: 1,
      },
      {
        id: "2",
        categoryId: "1",
        titleAr: "مطور Front End",
        titleEn: "Front End Developer",
        descriptionAr: "تطوير واجهات المستخدم",
        descriptionEn: "Develop user interfaces",
        displayOrder: 2,
      },
    ];
    localStorage.setItem("categoryJobs", JSON.stringify(sampleJobs));
  }

  if (!localStorage.getItem("categoryProjects")) {
    const sampleProjects = [
      {
        id: "1",
        categoryId: "1",
        titleAr: "متجر إلكتروني متكامل",
        titleEn: "Complete E-commerce Store",
        descriptionAr: "بناء متجر إلكتروني بجميع المميزات",
        descriptionEn: "Build a full-featured e-commerce store",
        image: "https://picsum.photos/seed/ecommerce/800/600.jpg",
        displayOrder: 1,
      },
      {
        id: "2",
        categoryId: "1",
        titleAr: "منصة تواصل اجتماعي",
        titleEn: "Social Media Platform",
        descriptionAr: "بناء منصة تواصل اجتماعي",
        descriptionEn: "Build a social media platform",
        image: "https://picsum.photos/seed/social/800/600.jpg",
        displayOrder: 2,
      },
    ];
    localStorage.setItem("categoryProjects", JSON.stringify(sampleProjects));
  }

  if (!localStorage.getItem("categoryTechnologies")) {
    const sampleTechnologies = [
      {
        id: "1",
        categoryId: "1",
        titleAr: "React",
        titleEn: "React",
        icon: "fa-react",
        displayOrder: 1,
      },
      {
        id: "2",
        categoryId: "1",
        titleAr: "Node.js",
        titleEn: "Node.js",
        icon: "fa-node-js",
        displayOrder: 2,
      },
      {
        id: "3",
        categoryId: "1",
        titleAr: "MongoDB",
        titleEn: "MongoDB",
        icon: "fa-database",
        displayOrder: 3,
      },
    ];
    localStorage.setItem("categoryTechnologies", JSON.stringify(sampleTechnologies));
  }

  if (!localStorage.getItem("courses")) {
    const sampleCourses = [
      {
        id: "1",
        titleAr: "مقدمة في HTML و CSS",
        titleEn: "Introduction to HTML & CSS",
        shortDescriptionAr: "تعلم أساسيات HTML و CSS",
        shortDescriptionEn: "Learn HTML & CSS basics",
        image: "https://picsum.photos/seed/htmlcss/800/600.jpg",
        duration: "20 ساعة",
        price: 500,
        isFree: false,
        currentState: 1,
        createdDate: new Date().toISOString(),
      },
      {
        id: "2",
        titleAr: "JavaScript المتقدم",
        titleEn: "Advanced JavaScript",
        shortDescriptionAr: "تعلم JavaScript المتقدم",
        shortDescriptionEn: "Learn advanced JavaScript",
        image: "https://picsum.photos/seed/javascript/800/600.jpg",
        duration: "30 ساعة",
        price: 800,
        isFree: false,
        currentState: 1,
        createdDate: new Date().toISOString(),
      },
    ];
    localStorage.setItem("courses", JSON.stringify(sampleCourses));
  }

  if (!localStorage.getItem("packageCourses")) {
    const samplePackageCourses = [
      {
        id: "1",
        packageId: "1",
        courseId: "1",
        displayOrder: 1,
      },
      {
        id: "2",
        packageId: "2",
        courseId: "1",
        displayOrder: 1,
      },
      {
        id: "3",
        packageId: "2",
        courseId: "2",
        displayOrder: 2,
      },
    ];
    localStorage.setItem("packageCourses", JSON.stringify(samplePackageCourses));
  }

  if (!localStorage.getItem("reviews")) {
    const sampleReviews = [
      {
        id: "1",
        reviewType: 1, // 1 = Diploma, 2 = Package
        referenceId: "1",
        fullName: "أحمد محمد",
        comment: "دبلوم ممتاز ومفيد جداً، ساعدني في تطوير مهاراتي بشكل كبير.",
        videoPath: "",
        imagePath: "https://randomuser.me/api/portraits/men/32.jpg",
        displayOrder: 1,
        currentState: 1,
        createdDate: new Date().toISOString(),
      },
      {
        id: "2",
        reviewType: 1,
        referenceId: "1",
        fullName: "سارة أحمد",
        comment: "المحتوى كان شاملاً والمدربين كانوا متخصصين، أنصح به بشدة.",
        videoPath: "",
        imagePath: "https://randomuser.me/api/portraits/women/44.jpg",
        displayOrder: 2,
        currentState: 1,
        createdDate: new Date().toISOString(),
      },
      {
        id: "3",
        reviewType: 2,
        referenceId: "1",
        fullName: "محمد علي",
        comment: "باقة رائعة تحتوي على جميع الدورات اللازمة للبدء في مجال تطوير الويب.",
        videoPath: "",
        imagePath: "https://randomuser.me/api/portraits/men/67.jpg",
        displayOrder: 1,
        currentState: 1,
        createdDate: new Date().toISOString(),
      },
    ];
    localStorage.setItem("reviews", JSON.stringify(sampleReviews));
  }
}

// Diplomas CRUD Functions
function loadDiplomas() {
  return JSON.parse(localStorage.getItem("diplomas") || "[]");
}

function saveDiploma(diploma) {
  const diplomas = loadDiplomas();
  if (diploma.id) {
    // Update existing
    const index = diplomas.findIndex((d) => d.id === diploma.id);
    if (index !== -1) {
      diplomas[index] = { ...diplomas[index], ...diploma };
    }
  } else {
    // Add new
    diploma.id = Date.now().toString();
    diploma.createdDate = new Date().toISOString();
    diplomas.push(diploma);
  }
  localStorage.setItem("diplomas", JSON.stringify(diplomas));
  return diploma;
}

function deleteDiploma(id) {
  const diplomas = loadDiplomas();
  const filtered = diplomas.filter((d) => d.id !== id);
  localStorage.setItem("diplomas", JSON.stringify(filtered));
}

function renderDiplomas(view = "grid") {
  const diplomas = loadDiplomas();
  const gridView = document.getElementById("diplomasGridView");
  const tableView = document.getElementById("diplomasTableView");
  const tableBody = document.getElementById("diplomasTableBody");

  // Update metrics
  document.getElementById("totalDiplomas").textContent = diplomas.length;
  document.getElementById("activeDiplomas").textContent = diplomas.filter((d) => d.currentState === 1).length;
  document.getElementById("bestSellerDiplomas").textContent = diplomas.filter((d) => d.isBestSeller).length;

  if (view === "grid") {
    gridView.style.display = "grid";
    tableView.style.display = "none";

    gridView.innerHTML = diplomas
      .map(
        (diploma) => `
      <div class="diploma-card" onclick="showDiplomaDetails('${diploma.id}')" style="cursor: pointer;">
        <img src="${diploma.image || "/placeholder-diploma.jpg"}" alt="${diploma.titleEn}" class="diploma-card-image">
        <div class="diploma-card-content">
          <div class="diploma-card-title">${currentLanguage === "ar" ? diploma.titleAr : diploma.titleEn}</div>
          <div class="diploma-card-description">${
            currentLanguage === "ar" ? diploma.shortDescriptionAr : diploma.shortDescriptionEn
          }</div>
          <div class="diploma-card-meta">
            <div class="diploma-card-code">كود: ${diploma.categoryCode}</div>
            <div class="diploma-card-badge ${diploma.isBestSeller ? "best-seller" : "active"}">
              ${diploma.isBestSeller ? "الأكثر مبيعاً" : "نشط"}
            </div>
          </div>
        </div>
        <div class="diploma-actions" onclick="event.stopPropagation()">
          <button class="btn btn-sm" onclick="editDiploma('${diploma.id}')">
            <i class="fas fa-edit"></i>
            <span data-i18n="btnEdit">تعديل</span>
          </button>
          <button class="btn btn-sm" onclick="deleteDiplomaConfirm('${diploma.id}')">
            <i class="fas fa-trash"></i>
            <span data-i18n="btnDelete">حذف</span>
          </button>
        </div>
      </div>
    `,
      )
      .join("");
  } else {
    gridView.style.display = "none";
    tableView.style.display = "block";

    tableBody.innerHTML = diplomas
      .map(
        (diploma) => `
      <tr onclick="showDiplomaDetails('${diploma.id}')" style="cursor: pointer;">
        <td>
          <img src="${diploma.image || "/placeholder-diploma.jpg"}" alt="${diploma.titleEn}" class="course-thumbnail" />
        </td>
        <td>${diploma.titleAr}</td>
        <td>${diploma.titleEn}</td>
        <td>${diploma.categoryCode}</td>
        <td>${diploma.priority}</td>
        <td>
          <span class="status-badge">${diploma.currentState === 1 ? "نشط" : "غير نشط"}</span>
        </td>
        <td onclick="event.stopPropagation()">
          <div class="table-actions">
            <button class="btn-table-action" onclick="editDiploma('${diploma.id}')">
              <i class="fas fa-edit"></i> تعديل
            </button>
            <button class="btn-table-action" onclick="deleteDiplomaConfirm('${diploma.id}')">
              <i class="fas fa-trash"></i> حذف
            </button>
          </div>
        </td>
      </tr>
    `,
      )
      .join("");
  }

  updateTranslations();
}

function showDiplomaForm(diplomaId = null) {
  const diploma = diplomaId ? loadDiplomas().find((d) => d.id === diplomaId) : null;

  const formHTML = `
    <div class="modal-overlay" id="diplomaModal" onclick="closeDiplomaModal(event)">
      <div class="modal-content large-modal" onclick="event.stopPropagation()">
        <div class="modal-header">
          <h2>${diploma ? "تعديل الدبلوم" : "إضافة دبلوم جديد"}</h2>
          <button class="btn-icon" onclick="closeDiplomaModal()">
            <i class="fas fa-times"></i>
          </button>
        </div>
        <form id="diplomaForm" class="modal-body">
          <input type="hidden" id="diplomaId" value="${diploma?.id || ""}" />
          
          <h3 class="form-section-title">المعلومات الأساسية</h3>
          <div class="form-row">
            <div class="form-group">
              <label for="titleAr">العنوان (عربي)</label>
              <input type="text" id="titleAr" value="${diploma?.titleAr || ""}" required />
            </div>
            <div class="form-group">
              <label for="titleEn">العنوان (إنجليزي)</label>
              <input type="text" id="titleEn" value="${diploma?.titleEn || ""}" required />
            </div>
          </div>

          <div class="form-row">
            <div class="form-group">
              <label for="categoryCode">كود الدبلوم</label>
              <input type="number" id="categoryCode" value="${diploma?.categoryCode || ""}" required />
            </div>
            <div class="form-group">
              <label for="priority">الأولوية</label>
              <input type="number" id="priority" value="${diploma?.priority || 1}" required />
            </div>
            <div class="form-group">
              <label for="categoryTypeId">نوع الدبلوم</label>
              <input type="number" id="categoryTypeId" value="${diploma?.categoryTypeId || 1}" required />
            </div>
          </div>

          <div class="form-row">
            <div class="form-group">
              <label for="shortDescriptionAr">الوصف المختصر (عربي)</label>
              <textarea id="shortDescriptionAr" rows="3">${diploma?.shortDescriptionAr || ""}</textarea>
            </div>
            <div class="form-group">
              <label for="shortDescriptionEn">الوصف المختصر (إنجليزي)</label>
              <textarea id="shortDescriptionEn" rows="3">${diploma?.shortDescriptionEn || ""}</textarea>
            </div>
          </div>

          <h3 class="form-section-title">معلومات الميزة الرئيسية</h3>
          <div class="form-row">
            <div class="form-group">
              <label for="featureTitleAr">عنوان الميزة (عربي)</label>
              <input type="text" id="featureTitleAr" value="${diploma?.featureTitleAr || ""}" />
            </div>
            <div class="form-group">
              <label for="featureTitleEn">عنوان الميزة (إنجليزي)</label>
              <input type="text" id="featureTitleEn" value="${diploma?.featureTitleEn || ""}" />
            </div>
          </div>

          <div class="form-row">
            <div class="form-group">
              <label for="featureShortDescriptionAr">وصف الميزة (عربي)</label>
              <textarea id="featureShortDescriptionAr" rows="3">${diploma?.featureShortDescriptionAr || ""}</textarea>
            </div>
            <div class="form-group">
              <label for="featureShortDescriptionEn">وصف الميزة (إنجليزي)</label>
              <textarea id="featureShortDescriptionEn" rows="3">${diploma?.featureShortDescriptionEn || ""}</textarea>
            </div>
          </div>

          <h3 class="form-section-title">إعدادات إضافية</h3>
          <div class="form-row">
            <div class="form-group">
              <label for="displayOrder">ترتيب العرض</label>
              <input type="number" id="displayOrder" value="${diploma?.displayOrder || 1}" />
            </div>
            <div class="form-group">
              <label for="isBestSeller">الأكثر مبيعاً</label>
              <select id="isBestSeller">
                <option value="true" ${diploma?.isBestSeller ? "selected" : ""}>نعم</option>
                <option value="false" ${!diploma?.isBestSeller ? "selected" : ""}>لا</option>
              </select>
            </div>
          </div>

          <div class="form-group">
            <label for="videoPath">رابط الفيديو</label>
            <input type="text" id="videoPath" value="${diploma?.videoPath || ""}" />
          </div>

          <div class="form-group">
            <label for="image">رابط الصورة</label>
            <input type="text" id="image" value="${diploma?.image || ""}" />
          </div>

          <div class="modal-footer">
            <button type="button" class="btn" onclick="closeDiplomaModal()">
              <span data-i18n="btnCancel">إلغاء</span>
            </button>
            <button type="submit" class="btn btn-primary">
              <i class="fas fa-save"></i>
              <span data-i18n="btnSave">حفظ</span>
            </button>
          </div>
        </form>
      </div>
    </div>
  `;

  document.body.insertAdjacentHTML("beforeend", formHTML);

  document.getElementById("diplomaForm").addEventListener("submit", (e) => {
    e.preventDefault();
    const formData = {
      id: document.getElementById("diplomaId").value,
      titleAr: document.getElementById("titleAr").value,
      titleEn: document.getElementById("titleEn").value,
      categoryCode: Number.parseInt(document.getElementById("categoryCode").value),
      priority: Number.parseInt(document.getElementById("priority").value),
      categoryTypeId: Number.parseInt(document.getElementById("categoryTypeId").value),
      shortDescriptionAr: document.getElementById("shortDescriptionAr").value,
      shortDescriptionEn: document.getElementById("shortDescriptionEn").value,
      featureTitleAr: document.getElementById("featureTitleAr").value,
      featureTitleEn: document.getElementById("featureTitleEn").value,
      featureShortDescriptionAr: document.getElementById("featureShortDescriptionAr").value,
      featureShortDescriptionEn: document.getElementById("featureShortDescriptionEn").value,
      displayOrder: Number.parseInt(document.getElementById("displayOrder").value),
      isBestSeller: document.getElementById("isBestSeller").value === "true",
      videoPath: document.getElementById("videoPath").value,
      image: document.getElementById("image").value || "/placeholder-diploma.jpg",
      currentState: 1,
    };
    saveDiploma(formData);
    closeDiplomaModal();
    renderDiplomas(currentDiplomasView);
  });

  updateTranslations();
}

function closeDiplomaModal(event) {
  if (event && event.target.classList.contains("modal-content")) return;
  const modal = document.getElementById("diplomaModal");
  if (modal) modal.remove();
}

function editDiploma(id) {
  showDiplomaForm(id);
}

function deleteDiplomaConfirm(id) {
  if (confirm("هل أنت متأكد من حذف هذا الدبلوم؟")) {
    deleteDiploma(id);
    renderDiplomas(currentDiplomasView);
  }
}

// Diploma Details Functions
function showDiplomaDetails(diplomaId) {
  currentDiplomaId = diplomaId;
  const diploma = loadDiplomas().find((d) => d.id === diplomaId);
  if (!diploma) return;

  showPage("diploma-details");

  // Update breadcrumb and title
  document.getElementById("diplomaDetailsBreadcrumb").textContent =
    currentLanguage === "ar" ? diploma.titleAr : diploma.titleEn;
  document.getElementById("diplomaDetailsTitle").textContent =
    currentLanguage === "ar" ? diploma.titleAr : diploma.titleEn;

  // Display diploma info with image
  const infoContent = document.getElementById("diplomaInfoContent");
  infoContent.innerHTML = `
    <div class="diploma-details-container">
      <div class="diploma-details-image">
        <img src="${diploma.image || "/placeholder-diploma.jpg"}" alt="${diploma.titleEn}" />
      </div>
      <div class="diploma-details-content">
        <div class="detail-row">
          <div class="detail-label">العنوان (عربي):</div>
          <div class="detail-value">${diploma.titleAr}</div>
        </div>
        <div class="detail-row">
          <div class="detail-label">العنوان (إنجليزي):</div>
          <div class="detail-value">${diploma.titleEn}</div>
        </div>
        <div class="detail-row">
          <div class="detail-label">كود الدبلوم:</div>
          <div class="detail-value">${diploma.categoryCode}</div>
        </div>
        <div class="detail-row">
          <div class="detail-label">الوصف المختصر (عربي):</div>
          <div class="detail-value">${diploma.shortDescriptionAr || "غير متوفر"}</div>
        </div>
        <div class="detail-row">
          <div class="detail-label">الوصف المختصر (إنجليزي):</div>
          <div class="detail-value">${diploma.shortDescriptionEn || "غير متوفر"}</div>
        </div>
        <div class="detail-row">
          <div class="detail-label">عنوان الميزة (عربي):</div>
          <div class="detail-value">${diploma.featureTitleAr || "غير متوفر"}</div>
        </div>
        <div class="detail-row">
          <div class="detail-label">عنوان الميزة (إنجليزي):</div>
          <div class="detail-value">${diploma.featureTitleEn || "غير متوفر"}</div>
        </div>
        <div class="detail-row">
          <div class="detail-label">وصف الميزة (عربي):</div>
          <div class="detail-value">${diploma.featureShortDescriptionAr || "غير متوفر"}</div>
        </div>
        <div class="detail-row">
          <div class="detail-label">وصف الميزة (إنجليزي):</div>
          <div class="detail-value">${diploma.featureShortDescriptionEn || "غير متوفر"}</div>
        </div>
        <div class="detail-row">
          <div class="detail-label">الأولوية:</div>
          <div class="detail-value">${diploma.priority}</div>
        </div>
        <div class="detail-row">
          <div class="detail-label">ترتيب العرض:</div>
          <div class="detail-value">${diploma.displayOrder}</div>
        </div>
        <div class="detail-row">
          <div class="detail-label">الأكثر مبيعاً:</div>
          <div class="detail-value">${diploma.isBestSeller ? "نعم" : "لا"}</div>
        </div>
        <div class="detail-row">
          <div class="detail-label">نوع الدبلوم:</div>
          <div class="detail-value">${diploma.categoryTypeId}</div>
        </div>
        ${
          diploma.videoPath
            ? `
        <div class="detail-row">
          <div class="detail-label">رابط الفيديو:</div>
          <div class="detail-value"><a href="${diploma.videoPath}" target="_blank">${diploma.videoPath}</a></div>
        </div>
        `
            : ""
        }
      </div>
    </div>
  `;

  // Render packages for this diploma
  renderDiplomaPackages(diplomaId);

  // Render features, jobs, projects, technologies
  renderCategoryFeatures(diplomaId);
  renderCategoryJobs(diplomaId);
  renderCategoryProjects(diplomaId);
  renderCategoryTechnologies(diplomaId);
  
  // Render reviews
  renderReviews(diplomaId);
}

function renderDiplomaPackages(diplomaId) {
  const packages = loadPackages().filter((p) => p.categoryId === diplomaId);
  const grid = document.getElementById("diplomaPackagesGrid");

  if (packages.length === 0) {
    grid.innerHTML = '<p class="empty-message">لا توجد باقات لهذا الدبلوم</p>';
    return;
  }

  grid.innerHTML = packages
    .map(
      (pkg) => `
    <div class="diploma-card" onclick="showPackageDetails('${pkg.id}')" style="cursor: pointer;">
      <div class="diploma-badge">
        <i class="fas fa-box"></i>
      </div>
      <div class="diploma-content">
        <div class="diploma-course">${currentLanguage === "ar" ? pkg.titleAr : pkg.titleEn}</div>
        <div class="diploma-student">${currentLanguage === "ar" ? pkg.shortDescriptionAr : pkg.shortDescriptionEn}</div>
        <div class="diploma-meta">
          <div class="diploma-date">
            <i class="fas fa-sort-numeric-up"></i>
            <span>الترتيب: ${pkg.rank}</span>
          </div>
          <div class="diploma-grade">
            <i class="fas fa-percent"></i>
            <span>خصم: ${pkg.generalDiscount}%</span>
          </div>
        </div>
      </div>
      <div class="diploma-actions" onclick="event.stopPropagation()">
        <button class="btn btn-sm" onclick="editPackage('${pkg.id}')">
          <i class="fas fa-edit"></i>
          <span>تعديل</span>
        </button>
        <button class="btn btn-sm" onclick="deletePackageConfirm('${pkg.id}')">
          <i class="fas fa-trash"></i>
          <span>حذف</span>
        </button>
      </div>
    </div>
  `,
    )
    .join("");
}

// Category Features CRUD
function loadCategoryFeatures() {
  return JSON.parse(localStorage.getItem("categoryFeatures") || "[]");
}

function saveCategoryFeature(feature) {
  const features = loadCategoryFeatures();
  if (feature.id) {
    const index = features.findIndex((f) => f.id === feature.id);
    if (index !== -1) features[index] = { ...features[index], ...feature };
  } else {
    feature.id = Date.now().toString();
    feature.createdDate = new Date().toISOString();
    features.push(feature);
  }
  localStorage.setItem("categoryFeatures", JSON.stringify(features));
  return feature;
}

function deleteCategoryFeature(id) {
  const features = loadCategoryFeatures();
  localStorage.setItem("categoryFeatures", JSON.stringify(features.filter((f) => f.id !== id)));
}

function renderCategoryFeatures(categoryId) {
  const features = loadCategoryFeatures().filter((f) => f.categoryId === categoryId);
  const grid = document.getElementById("featuresGrid");

  if (features.length === 0) {
    grid.innerHTML = '<p class="empty-message">لا توجد مميزات لهذا الدبلوم</p>';
    return;
  }

  grid.innerHTML = features
    .map(
      (feature) => `
    <div class="feature-card">
      <div class="feature-icon">
        <i class="${feature.iconPath || "fas fa-star"}"></i>
      </div>
      <div class="feature-content">
        <div class="feature-title">${currentLanguage === "ar" ? feature.titleAr : feature.titleEn}</div>
      </div>
      <div class="feature-actions">
        <button class="btn-icon" onclick="editFeature('${feature.id}')">
          <i class="fas fa-edit"></i>
        </button>
        <button class="btn-icon" onclick="deleteFeatureConfirm('${feature.id}')">
          <i class="fas fa-trash"></i>
        </button>
      </div>
    </div>
  `,
    )
    .join("");
}

function showFeatureForm(featureId = null) {
  const feature = featureId ? loadCategoryFeatures().find((f) => f.id === featureId) : null;

  const formHTML = `
    <div class="modal-overlay" id="featureModal" onclick="closeFeatureModal(event)">
      <div class="modal-content" onclick="event.stopPropagation()">
        <div class="modal-header">
          <h2>${feature ? "تعديل الميزة" : "إضافة ميزة جديدة"}</h2>
          <button class="btn-icon" onclick="closeFeatureModal()">
            <i class="fas fa-times"></i>
          </button>
        </div>
        <form id="featureForm" class="modal-body">
          <input type="hidden" id="featureId" value="${feature?.id || ""}" />
          
          <div class="form-row">
            <div class="form-group">
              <label for="featureTitleAr">العنوان (عربي)</label>
              <input type="text" id="featureTitleAr" value="${feature?.titleAr || ""}" required />
            </div>
            <div class="form-group">
              <label for="featureTitleEn">العنوان (إنجليزي)</label>
              <input type="text" id="featureTitleEn" value="${feature?.titleEn || ""}" required />
            </div>
          </div>

          <div class="form-group">
            <label for="iconPath">أيقونة (Font Awesome class)</label>
            <input type="text" id="iconPath" value="${feature?.iconPath || "fas fa-star"}" placeholder="fas fa-star" />
          </div>

          <div class="modal-footer">
            <button type="button" class="btn" onclick="closeFeatureModal()">
              <span>إلغاء</span>
            </button>
            <button type="submit" class="btn btn-primary">
              <i class="fas fa-save"></i>
              <span>حفظ</span>
            </button>
          </div>
        </form>
      </div>
    </div>
  `;

  document.body.insertAdjacentHTML("beforeend", formHTML);

  document.getElementById("featureForm").addEventListener("submit", (e) => {
    e.preventDefault();
    const formData = {
      id: document.getElementById("featureId").value,
      categoryId: currentDiplomaId,
      titleAr: document.getElementById("featureTitleAr").value,
      titleEn: document.getElementById("featureTitleEn").value,
      iconPath: document.getElementById("iconPath").value,
    };
    saveCategoryFeature(formData);
    closeFeatureModal();
    renderCategoryFeatures(currentDiplomaId);
  });
}

function closeFeatureModal(event) {
  if (event && event.target.classList.contains("modal-content")) return;
  const modal = document.getElementById("featureModal");
  if (modal) modal.remove();
}

function editFeature(id) {
  showFeatureForm(id);
}

function deleteFeatureConfirm(id) {
  if (confirm("هل أنت متأكد من حذف هذه الميزة؟")) {
    deleteCategoryFeature(id);
    renderCategoryFeatures(currentDiplomaId);
  }
}

// Category Jobs CRUD
function loadCategoryJobs() {
  return JSON.parse(localStorage.getItem("categoryJobs") || "[]");
}

function saveCategoryJob(job) {
  const jobs = loadCategoryJobs();
  if (job.id) {
    const index = jobs.findIndex((j) => j.id === job.id);
    if (index !== -1) jobs[index] = { ...jobs[index], ...job };
  } else {
    job.id = Date.now().toString();
    job.createdDate = new Date().toISOString();
    jobs.push(job);
  }
  localStorage.setItem("categoryJobs", JSON.stringify(jobs));
  return job;
}

function deleteCategoryJob(id) {
  const jobs = loadCategoryJobs();
  localStorage.setItem("categoryJobs", JSON.stringify(jobs.filter((j) => j.id !== id)));
}

function renderCategoryJobs(categoryId) {
  const jobs = loadCategoryJobs().filter((j) => j.categoryId === categoryId);
  const list = document.getElementById("jobsList");

  if (jobs.length === 0) {
    list.innerHTML = '<p class="empty-message">لا توجد وظائف لهذا الدبلوم</p>';
    return;
  }

  list.innerHTML = jobs
    .map(
      (job) => `
    <div class="job-item">
      <div class="job-content">
        <div class="job-icon">
          <i class="fas fa-briefcase"></i>
        </div>
        <div class="job-title">${job.title}</div>
      </div>
      <div class="job-actions">
        <button class="btn-icon" onclick="editJob('${job.id}')">
          <i class="fas fa-edit"></i>
        </button>
        <button class="btn-icon" onclick="deleteJobConfirm('${job.id}')">
          <i class="fas fa-trash"></i>
        </button>
      </div>
    </div>
  `,
    )
    .join("");
}

function showJobForm(jobId = null) {
  const job = jobId ? loadCategoryJobs().find((j) => j.id === jobId) : null;

  const formHTML = `
    <div class="modal-overlay" id="jobModal" onclick="closeJobModal(event)">
      <div class="modal-content" onclick="event.stopPropagation()">
        <div class="modal-header">
          <h2>${job ? "تعديل الوظيفة" : "إضافة وظيفة جديدة"}</h2>
          <button class="btn-icon" onclick="closeJobModal()">
            <i class="fas fa-times"></i>
          </button>
        </div>
        <form id="jobForm" class="modal-body">
          <input type="hidden" id="jobId" value="${job?.id || ""}" />
          
          <div class="form-group">
            <label for="jobTitle">عنوان الوظيفة</label>
            <input type="text" id="jobTitle" value="${job?.title || ""}" required />
          </div>

          <div class="form-group">
            <label for="jobDisplayOrder">ترتيب العرض</label>
            <input type="number" id="jobDisplayOrder" value="${job?.displayOrder || 1}" required />
          </div>

          <div class="modal-footer">
            <button type="button" class="btn" onclick="closeJobModal()">
              <span>إلغاء</span>
            </button>
            <button type="submit" class="btn btn-primary">
              <i class="fas fa-save"></i>
              <span>حفظ</span>
            </button>
          </div>
        </form>
      </div>
    </div>
  `;

  document.body.insertAdjacentHTML("beforeend", formHTML);

  document.getElementById("jobForm").addEventListener("submit", (e) => {
    e.preventDefault();
    const formData = {
      id: document.getElementById("jobId").value,
      categoryId: currentDiplomaId,
      title: document.getElementById("jobTitle").value,
      displayOrder: Number.parseInt(document.getElementById("jobDisplayOrder").value),
    };
    saveCategoryJob(formData);
    closeJobModal();
    renderCategoryJobs(currentDiplomaId);
  });
}

function closeJobModal(event) {
  if (event && event.target.classList.contains("modal-content")) return;
  const modal = document.getElementById("jobModal");
  if (modal) modal.remove();
}

function editJob(id) {
  showJobForm(id);
}

function deleteJobConfirm(id) {
  if (confirm("هل أنت متأكد من حذف هذه الوظيفة؟")) {
    deleteCategoryJob(id);
    renderCategoryJobs(currentDiplomaId);
  }
}

// Category Projects CRUD
function loadCategoryProjects() {
  return JSON.parse(localStorage.getItem("categoryProjects") || "[]");
}

function saveCategoryProject(project) {
  const projects = loadCategoryProjects();
  if (project.id) {
    const index = projects.findIndex((p) => p.id === project.id);
    if (index !== -1) projects[index] = { ...projects[index], ...project };
  } else {
    project.id = Date.now().toString();
    project.createdDate = new Date().toISOString();
    projects.push(project);
  }
  localStorage.setItem("categoryProjects", JSON.stringify(projects));
  return project;
}

function deleteCategoryProject(id) {
  const projects = loadCategoryProjects();
  localStorage.setItem("categoryProjects", JSON.stringify(projects.filter((p) => p.id !== id)));
}

function renderCategoryProjects(categoryId) {
  const projects = loadCategoryProjects().filter((p) => p.categoryId === categoryId);
  const grid = document.getElementById("projectsGrid");

  if (projects.length === 0) {
    grid.innerHTML = '<p class="empty-message">لا توجد مشاريع لهذا الدبلوم</p>';
    return;
  }

  grid.innerHTML = projects
    .map(
      (project) => `
    <div class="diploma-card">
      <div class="diploma-badge">
        <i class="fas fa-project-diagram"></i>
      </div>
      <div class="diploma-content">
        <div class="diploma-course">${currentLanguage === "ar" ? project.titleAr : project.titleEn}</div>
        <div class="diploma-student">${currentLanguage === "ar" ? project.shortDescriptionAr : project.shortDescriptionEn}</div>
        <div class="diploma-meta">
          <div class="diploma-date">
            <i class="fas fa-sort-numeric-up"></i>
            <span>الترتيب: ${project.displayOrder}</span>
          </div>
        </div>
      </div>
      <div class="diploma-actions">
        <button class="btn btn-sm" onclick="editProject('${project.id}')">
          <i class="fas fa-edit"></i>
          <span>تعديل</span>
        </button>
        <button class="btn btn-sm" onclick="deleteProjectConfirm('${project.id}')">
          <i class="fas fa-trash"></i>
          <span>حذف</span>
        </button>
      </div>
    </div>
  `,
    )
    .join("");
}

function showProjectForm(projectId = null) {
  const project = projectId ? loadCategoryProjects().find((p) => p.id === projectId) : null;

  const formHTML = `
    <div class="modal-overlay" id="projectModal" onclick="closeProjectModal(event)">
      <div class="modal-content" onclick="event.stopPropagation()">
        <div class="modal-header">
          <h2>${project ? "تعديل المشروع" : "إضافة مشروع جديد"}</h2>
          <button class="btn-icon" onclick="closeProjectModal()">
            <i class="fas fa-times"></i>
          </button>
        </div>
        <form id="projectForm" class="modal-body">
          <input type="hidden" id="projectId" value="${project?.id || ""}" />
          
          <div class="form-row">
            <div class="form-group">
              <label for="projectTitleAr">العنوان (عربي)</label>
              <input type="text" id="projectTitleAr" value="${project?.titleAr || ""}" required />
            </div>
            <div class="form-group">
              <label for="projectTitleEn">العنوان (إنجليزي)</label>
              <input type="text" id="projectTitleEn" value="${project?.titleEn || ""}" required />
            </div>
          </div>

          <div class="form-row">
            <div class="form-group">
              <label for="projectShortDescriptionAr">الوصف (عربي)</label>
              <textarea id="projectShortDescriptionAr" rows="3">${project?.shortDescriptionAr || ""}</textarea>
            </div>
            <div class="form-group">
              <label for="projectShortDescriptionEn">الوصف (إنجليزي)</label>
              <textarea id="projectShortDescriptionEn" rows="3">${project?.shortDescriptionEn || ""}</textarea>
            </div>
          </div>

          <div class="form-row">
            <div class="form-group">
              <label for="projectImagePath">رابط الصورة</label>
              <input type="text" id="projectImagePath" value="${project?.imagePath || ""}" />
            </div>
            <div class="form-group">
              <label for="projectVideoPath">رابط الفيديو</label>
              <input type="text" id="projectVideoPath" value="${project?.videoPath || ""}" />
            </div>
          </div>

          <div class="form-group">
            <label for="projectDisplayOrder">ترتيب العرض</label>
            <input type="number" id="projectDisplayOrder" value="${project?.displayOrder || 1}" required />
          </div>

          <div class="modal-footer">
            <button type="button" class="btn" onclick="closeProjectModal()">
              <span>إلغاء</span>
            </button>
            <button type="submit" class="btn btn-primary">
              <i class="fas fa-save"></i>
              <span>حفظ</span>
            </button>
          </div>
        </form>
      </div>
    </div>
  `;

  document.body.insertAdjacentHTML("beforeend", formHTML);

  document.getElementById("projectForm").addEventListener("submit", (e) => {
    e.preventDefault();
    const formData = {
      id: document.getElementById("projectId").value,
      categoryId: currentDiplomaId,
      titleAr: document.getElementById("projectTitleAr").value,
      titleEn: document.getElementById("projectTitleEn").value,
      shortDescriptionAr: document.getElementById("projectShortDescriptionAr").value,
      shortDescriptionEn: document.getElementById("projectShortDescriptionEn").value,
      imagePath: document.getElementById("projectImagePath").value,
      videoPath: document.getElementById("projectVideoPath").value,
      displayOrder: Number.parseInt(document.getElementById("projectDisplayOrder").value),
    };
    saveCategoryProject(formData);
    closeProjectModal();
    renderCategoryProjects(currentDiplomaId);
  });
}

function closeProjectModal(event) {
  if (event && event.target.classList.contains("modal-content")) return;
  const modal = document.getElementById("projectModal");
  if (modal) modal.remove();
}

function editProject(id) {
  showProjectForm(id);
}

function deleteProjectConfirm(id) {
  if (confirm("هل أنت متأكد من حذف هذا المشروع؟")) {
    deleteCategoryProject(id);
    renderCategoryProjects(currentDiplomaId);
  }
}

// Category Technologies CRUD
function loadCategoryTechnologies() {
  return JSON.parse(localStorage.getItem("categoryTechnologies") || "[]");
}

function saveCategoryTechnology(tech) {
  const technologies = loadCategoryTechnologies();
  if (tech.id) {
    const index = technologies.findIndex((t) => t.id === tech.id);
    if (index !== -1) technologies[index] = { ...technologies[index], ...tech };
  } else {
    tech.id = Date.now().toString();
    tech.createdDate = new Date().toISOString();
    technologies.push(tech);
  }
  localStorage.setItem("categoryTechnologies", JSON.stringify(technologies));
  return tech;
}

function deleteCategoryTechnology(id) {
  const technologies = loadCategoryTechnologies();
  localStorage.setItem("categoryTechnologies", JSON.stringify(technologies.filter((t) => t.id !== id)));
}

function renderCategoryTechnologies(categoryId) {
  const technologies = loadCategoryTechnologies().filter((t) => t.categoryId === categoryId);
  const grid = document.getElementById("technologiesGrid");

  if (technologies.length === 0) {
    grid.innerHTML = '<p class="empty-message">لا توجد تقنيات لهذا الدبلوم</p>';
    return;
  }

  grid.innerHTML = technologies
    .map(
      (tech) => `
    <div class="technology-card">
      <div class="technology-image">
        <img src="${tech.imagePath || "/placeholder-tech.jpg"}" alt="${tech.title}" />
      </div>
      <div class="technology-title">${tech.title}</div>
      <div class="technology-actions">
        <button class="btn-icon" onclick="editTechnology('${tech.id}')">
          <i class="fas fa-edit"></i>
        </button>
        <button class="btn-icon" onclick="deleteTechnologyConfirm('${tech.id}')">
          <i class="fas fa-trash"></i>
        </button>
      </div>
    </div>
  `,
    )
    .join("");
}

function showTechnologyForm(techId = null) {
  const tech = techId ? loadCategoryTechnologies().find((t) => t.id === techId) : null;

  const formHTML = `
    <div class="modal-overlay" id="technologyModal" onclick="closeTechnologyModal(event)">
      <div class="modal-content" onclick="event.stopPropagation()">
        <div class="modal-header">
          <h2>${tech ? "تعديل التقنية" : "إضافة تقنية جديدة"}</h2>
          <button class="btn-icon" onclick="closeTechnologyModal()">
            <i class="fas fa-times"></i>
          </button>
        </div>
        <form id="technologyForm" class="modal-body">
          <input type="hidden" id="technologyId" value="${tech?.id || ""}" />
          
          <div class="form-group">
            <label for="technologyTitle">اسم التقنية</label>
            <input type="text" id="technologyTitle" value="${tech?.title || ""}" required />
          </div>

          <div class="form-group">
            <label for="technologyImagePath">رابط الصورة</label>
            <input type="text" id="technologyImagePath" value="${tech?.imagePath || ""}" />
          </div>

          <div class="form-group">
            <label for="technologyDisplayOrder">ترتيب العرض</label>
            <input type="number" id="technologyDisplayOrder" value="${tech?.displayOrder || 1}" required />
          </div>

          <div class="modal-footer">
            <button type="button" class="btn" onclick="closeTechnologyModal()">
              <span>إلغاء</span>
            </button>
            <button type="submit" class="btn btn-primary">
              <i class="fas fa-save"></i>
              <span>حفظ</span>
            </button>
          </div>
        </form>
      </div>
    </div>
  `;

  document.body.insertAdjacentHTML("beforeend", formHTML);

  document.getElementById("technologyForm").addEventListener("submit", (e) => {
    e.preventDefault();
    const formData = {
      id: document.getElementById("technologyId").value,
      categoryId: currentDiplomaId,
      title: document.getElementById("technologyTitle").value,
      imagePath: document.getElementById("technologyImagePath").value,
      displayOrder: Number.parseInt(document.getElementById("technologyDisplayOrder").value),
    };
    saveCategoryTechnology(formData);
    closeTechnologyModal();
    renderCategoryTechnologies(currentDiplomaId);
  });
}

function closeTechnologyModal(event) {
  if (event && event.target.classList.contains("modal-content")) return;
  const modal = document.getElementById("technologyModal");
  if (modal) modal.remove();
}

function editTechnology(id) {
  showTechnologyForm(id);
}

function deleteTechnologyConfirm(id) {
  if (confirm("هل أنت متأكد من حذف هذه التقنية؟")) {
    deleteCategoryTechnology(id);
    renderCategoryTechnologies(currentDiplomaId);
  }
}

// Reviews CRUD Functions
function loadReviews() {
  return JSON.parse(localStorage.getItem("reviews") || "[]");
}

function saveReview(review) {
  const reviews = loadReviews();
  if (review.id) {
    const index = reviews.findIndex((r) => r.id === review.id);
    if (index !== -1) reviews[index] = { ...reviews[index], ...review };
  } else {
    review.id = Date.now().toString();
    review.createdDate = new Date().toISOString();
    review.referenceId = currentDiplomaId; // Link review to current diploma
    reviews.push(review);
  }
  localStorage.setItem("reviews", JSON.stringify(reviews));
  return review;
}

function deleteReview(id) {
  const reviews = loadReviews();
  localStorage.setItem("reviews", JSON.stringify(reviews.filter((r) => r.id !== id)));
}

function renderReviews(categoryId) {
  const reviews = loadReviews().filter((r) => r.referenceId === categoryId && r.reviewType === 1);
  const grid = document.getElementById("reviewsGrid");

  if (reviews.length === 0) {
    grid.innerHTML = '<p class="empty-message">لا توجد تقييمات لهذا الدبلوم</p>';
    return;
  }

  grid.innerHTML = reviews
    .map(
      (review) => `
    <div class="review-card">
      <div class="review-header">
        <img src="${review.imagePath || "/placeholder-avatar.jpg"}" alt="${review.fullName}" class="review-avatar">
        <div class="review-info">
          <div class="review-name">${review.fullName}</div>
          <div class="review-date">${new Date(review.createdDate).toLocaleDateString(currentLanguage === "ar" ? "ar-EG" : "en-US")}</div>
        </div>
      </div>
      <div class="review-rating">
        ${Array.from({ length: 5 }, (_, i) => 
          `<i class="fas fa-star${i < 4 ? "" : "-o"}"></i>`
        ).join("")}
      </div>
      <div class="review-content">${review.comment}</div>
      ${review.videoPath ? 
        `<video src="${review.videoPath}" class="review-video" controls></video>` : 
        ""
      }
      <div class="review-actions">
        <button class="btn-icon" onclick="editReview('${review.id}')">
          <i class="fas fa-edit"></i>
        </button>
        <button class="btn-icon" onclick="deleteReviewConfirm('${review.id}')">
          <i class="fas fa-trash"></i>
        </button>
      </div>
    </div>
  `,
    )
    .join("");
}

function showReviewForm(reviewId = null) {
  const review = reviewId ? loadReviews().find((r) => r.id === reviewId) : null;

  const formHTML = `
    <div class="modal-overlay" id="reviewModal" onclick="closeReviewModal(event)">
      <div class="modal-content" onclick="event.stopPropagation()">
        <div class="modal-header">
          <h2>${review ? "تعديل التقييم" : "إضافة تقييم جديد"}</h2>
          <button class="btn-icon" onclick="closeReviewModal()">
            <i class="fas fa-times"></i>
          </button>
        </div>
        <form id="reviewForm" class="modal-body">
          <input type="hidden" id="reviewId" value="${review?.id || ""}" />
          
          <div class="form-group">
            <label for="reviewFullName">الاسم</label>
            <input type="text" id="reviewFullName" value="${review?.fullName || ""}" required />
          </div>

          <div class="form-group">
            <label for="reviewComment">التعليق</label>
            <textarea id="reviewComment" rows="4" required>${review?.comment || ""}</textarea>
          </div>

          <div class="form-group">
            <label for="reviewVideoPath">رابط الفيديو (اختياري)</label>
            <input type="text" id="reviewVideoPath" value="${review?.videoPath || ""}" />
          </div>

          <div class="form-group">
            <label for="reviewImagePath">رابط الصورة (اختياري)</label>
            <input type="text" id="reviewImagePath" value="${review?.imagePath || ""}" />
          </div>

          <div class="form-group">
            <label for="reviewDisplayOrder">ترتيب العرض</label>
            <input type="number" id="reviewDisplayOrder" value="${review?.displayOrder || 1}" required />
          </div>

          <div class="modal-footer">
            <button type="button" class="btn" onclick="closeReviewModal()">
              <span>إلغاء</span>
            </button>
            <button type="submit" class="btn btn-primary">
              <i class="fas fa-save"></i>
              <span>حفظ</span>
            </button>
          </div>
        </form>
      </div>
    </div>
  `;

  document.body.insertAdjacentHTML("beforeend", formHTML);

  document.getElementById("reviewForm").addEventListener("submit", (e) => {
    e.preventDefault();
    const formData = {
      id: document.getElementById("reviewId").value,
      reviewType: 1, // Diploma review
      referenceId: currentDiplomaId,
      fullName: document.getElementById("reviewFullName").value,
      comment: document.getElementById("reviewComment").value,
      videoPath: document.getElementById("reviewVideoPath").value,
      imagePath: document.getElementById("reviewImagePath").value,
      displayOrder: Number.parseInt(document.getElementById("reviewDisplayOrder").value),
      currentState: 1,
    };
    saveReview(formData);
    closeReviewModal();
    renderReviews(currentDiplomaId);
  });
}

function closeReviewModal(event) {
  if (event && event.target.classList.contains("modal-content")) return;
  const modal = document.getElementById("reviewModal");
  if (modal) modal.remove();
}

function editReview(id) {
  showReviewForm(id);
}

function deleteReviewConfirm(id) {
  if (confirm("هل أنت متأكد من حذف هذا التقييم؟")) {
    deleteReview(id);
    renderReviews(currentDiplomaId);
  }
}

// Packages CRUD Functions
function loadPackages() {
  const packages = JSON.parse(localStorage.getItem("packages") || "[]");
  return packages;
}

function savePackage(pkg) {
  const packages = loadPackages();
  if (pkg.id) {
    const index = packages.findIndex((p) => p.id === pkg.id);
    if (index !== -1) {
      packages[index] = { ...packages[index], ...pkg };
    }
  } else {
    pkg.id = Date.now().toString();
    pkg.createdDate = new Date().toISOString();
    packages.push(pkg);
  }
  localStorage.setItem("packages", JSON.stringify(packages));
  return pkg;
}

function deletePackage(id) {
  const packages = loadPackages();
  const filtered = packages.filter((p) => p.id !== id);
  localStorage.setItem("packages", JSON.stringify(filtered));
}

function renderPackages(view = "grid") {
  const packages = loadPackages().filter(p => p.categoryId === currentDiplomaId);
  const gridView = document.getElementById("packagesGridView");
  const tableView = document.getElementById("packagesTableView");
  const tableBody = document.getElementById("packagesTableBody");

  // Update metrics
  document.getElementById("totalPackages").textContent = packages.length;
  document.getElementById("activePackages").textContent = packages.filter((p) => p.currentState === 1).length;
  document.getElementById("defaultPackages").textContent = packages.filter((p) => p.isDefault).length;

  if (view === "grid") {
    gridView.style.display = "grid";
    tableView.style.display = "none";

    gridView.innerHTML = packages
      .map((pkg) => {
        const packageCourses = JSON.parse(localStorage.getItem("packageCourses") || "[]")
          .filter(pc => pc.packageId === pkg.id)
          .map(pc => loadCourses().find(c => c.id === pc.courseId))
          .filter(Boolean);
        
        return `
      <div class="package-card">
        <div class="package-card-header">
          <div class="package-card-title">${currentLanguage === "ar" ? pkg.titleAr : pkg.titleEn}</div>
          <div class="package-card-description">${currentLanguage === "ar" ? pkg.shortDescriptionAr : pkg.shortDescriptionEn}</div>
        </div>
        <div class="package-card-body">
          <div class="package-price">${pkg.generalDiscount}% ${currentLanguage === "ar" ? "خصم" : "discount"}</div>
          <div class="package-features">
            <div class="package-feature">
              <i class="fas fa-check"></i>
              <span>${currentLanguage === "ar" ? "ترتيب" : "Rank"}: ${pkg.rank}</span>
            </div>
            <div class="package-feature">
              <i class="fas fa-check"></i>
              <span>${currentLanguage === "ar" ? "خصم الترقية" : "Upgrade discount"}: ${pkg.upgradeDiscount}%</span>
            </div>
            <div class="package-feature">
              <i class="fas fa-check"></i>
              <span>${currentLanguage === "ar" ? "معدل الخصم الافتراضي" : "Default discount rate"}: ${pkg.defaultDiscountRate}%</span>
            </div>
          </div>
          <div class="package-courses">
            <div class="package-courses-title">${currentLanguage === "ar" ? "الدورات المشمولة" : "Included courses"} (${packageCourses.length})</div>
            ${packageCourses.map(course => `
              <div class="package-course-item">
                <i class="fas fa-book"></i>
                <span>${currentLanguage === "ar" ? course.titleAr : course.titleEn}</span>
              </div>
            `).join("")}
          </div>
        </div>
        <div class="package-card-footer">
          <div class="package-badge ${pkg.isDefault ? "default" : ""} ${pkg.isLastPackage ? "last" : ""}">
            ${pkg.isDefault ? (currentLanguage === "ar" ? "افتراضي" : "Default") : ""}
            ${pkg.isLastPackage ? (currentLanguage === "ar" ? "الأخير" : "Last") : ""}
          </div>
          <div class="package-actions">
            <button class="btn btn-sm" onclick="showPackageDetails('${pkg.id}')">
              <i class="fas fa-eye"></i>
              <span>${currentLanguage === "ar" ? "تفاصيل" : "Details"}</span>
            </button>
            <button class="btn btn-sm" onclick="editPackage('${pkg.id}')">
              <i class="fas fa-edit"></i>
              <span>${currentLanguage === "ar" ? "تعديل" : "Edit"}</span>
            </button>
            <button class="btn btn-sm" onclick="deletePackageConfirm('${pkg.id}')">
              <i class="fas fa-trash"></i>
              <span>${currentLanguage === "ar" ? "حذف" : "Delete"}</span>
            </button>
          </div>
        </div>
      </div>
    `;
      })
      .join("");
  } else {
    gridView.style.display = "none";
    tableView.style.display = "block";

    tableBody.innerHTML = packages
      .map((pkg) => {
        return `
      <tr>
        <td>${pkg.titleAr}</td>
        <td>${pkg.titleEn}</td>
        <td>${pkg.rank}</td>
        <td>${pkg.generalDiscount}%</td>
        <td>
          <span class="status-badge">${pkg.isDefault ? "نعم" : "لا"}</span>
        </td>
        <td>
          <span class="status-badge">${pkg.currentState === 1 ? "نشط" : "غير نشط"}</span>
        </td>
        <td>
          <div class="table-actions">
            <button class="btn-table-action" onclick="showPackageDetails('${pkg.id}')">
              <i class="fas fa-eye"></i> تفاصيل
            </button>
            <button class="btn-table-action" onclick="editPackage('${pkg.id}')">
              <i class="fas fa-edit"></i> تعديل
            </button>
            <button class="btn-table-action" onclick="deletePackageConfirm('${pkg.id}')">
              <i class="fas fa-trash"></i> حذف
            </button>
          </div>
        </td>
      </tr>
    `;
      })
      .join("");
  }

  updateTranslations();
}

function showPackageFormWithCourses(packageId = null, categoryId = null) {
  const pkg = packageId ? loadPackages().find((p) => p.id === packageId) : null;
  const diplomas = loadDiplomas();
  const courses = loadCourses();
  const packageCourses = JSON.parse(localStorage.getItem("packageCourses") || "[]");
  const selectedCourses = packageId
    ? packageCourses.filter((pc) => pc.packageId === packageId).map((pc) => pc.courseId)
    : [];

  const formHTML = `
    <div class="modal-overlay" id="packageModal" onclick="closePackageModal(event)">
      <div class="modal-content large-modal" onclick="event.stopPropagation()">
        <div class="modal-header">
          <h2>${pkg ? "تعديل الباقة" : "إضافة باقة جديدة"}</h2>
          <button class="btn-icon" onclick="closePackageModal()">
            <i class="fas fa-times"></i>
          </button>
        </div>
        <form id="packageForm" class="modal-body">
          <input type="hidden" id="packageId" value="${pkg?.id || ""}" />
          
          <h3 class="form-section-title">المعلومات الأساسية</h3>
          <div class="form-row">
            <div class="form-group">
              <label for="pkgTitleAr">العنوان (عربي)</label>
              <input type="text" id="pkgTitleAr" value="${pkg?.titleAr || ""}" required />
            </div>
            <div class="form-group">
              <label for="pkgTitleEn">العنوان (إنجليزي)</label>
              <input type="text" id="pkgTitleEn" value="${pkg?.titleEn || ""}" required />
            </div>
          </div>

          <div class="form-group">
            <label for="categoryId">الدبلوم</label>
            <select id="categoryId" required ${categoryId ? "disabled" : ""}>
              <option value="">اختر الدبلوم</option>
              ${diplomas
                .map(
                  (d) => `
                <option value="${d.id}" ${pkg?.categoryId === d.id || categoryId === d.id ? "selected" : ""}>
                  ${currentLanguage === "ar" ? d.titleAr : d.titleEn}
                </option>
              `,
                )
                .join("")}
            </select>
            ${categoryId ? `<input type="hidden" id="categoryIdHidden" value="${categoryId}" />` : ""}
          </div>

          <div class="form-row">
            <div class="form-group">
              <label for="pkgShortDescriptionAr">الوصف المختصر (عربي)</label>
              <textarea id="pkgShortDescriptionAr" rows="3">${pkg?.shortDescriptionAr || ""}</textarea>
            </div>
            <div class="form-group">
              <label for="pkgShortDescriptionEn">الوصف المختصر (إنجليزي)</label>
              <textarea id="pkgShortDescriptionEn" rows="3">${pkg?.shortDescriptionEn || ""}</textarea>
            </div>
          </div>

          <h3 class="form-section-title">إعدادات الباقة</h3>
          <div class="form-row">
            <div class="form-group">
              <label for="rank">الترتيب</label>
              <input type="number" id="rank" value="${pkg?.rank || 1}" required />
            </div>
            <div class="form-group">
              <label for="generalDiscount">الخصم العام (%)</label>
              <input type="number" id="generalDiscount" value="${pkg?.generalDiscount || 0}" step="0.01" />
            </div>
          </div>

          <div class="form-row">
            <div class="form-group">
              <label for="upgradeDiscount">خصم الترقية (%)</label>
              <input type="number" id="upgradeDiscount" value="${pkg?.upgradeDiscount || 0}" step="0.01" />
            </div>
            <div class="form-group">
              <label for="defaultDiscountRate">معدل الخصم الافتراضي (%)</label>
              <input type="number" id="defaultDiscountRate" value="${pkg?.defaultDiscountRate || 0}" step="0.01" />
            </div>
          </div>

          <div class="form-row">
            <div class="form-group">
              <label for="isDefault">باقة افتراضية</label>
              <select id="isDefault">
                <option value="true" ${pkg?.isDefault ? "selected" : ""}>نعم</option>
                <option value="false" ${!pkg?.isDefault ? "selected" : ""}>لا</option>
              </select>
            </div>
            <div class="form-group">
              <label for="isLastPackage">آخر باقة</label>
              <select id="isLastPackage">
                <option value="true" ${pkg?.isLastPackage ? "selected" : ""}>نعم</option>
                <option value="false" ${!pkg?.isLastPackage ? "selected" : ""}>لا</option>
              </select>
            </div>
          </div>

          <h3 class="form-section-title">الدورات المرتبطة</h3>
          <div class="form-group">
            <label>اختر الدورات</label>
            <div class="courses-checklist">
              ${courses
                .map(
                  (course) => `
                <label class="checkbox-label">
                  <input type="checkbox" name="packageCourses" value="${course.id}" ${selectedCourses.includes(course.id) ? "checked" : ""} />
                  <span>${currentLanguage === "ar" ? course.titleAr : course.titleEn}</span>
                </label>
              `,
                )
                .join("")}
            </div>
          </div>

          <div class="modal-footer">
            <button type="button" class="btn" onclick="closePackageModal()">
              <span data-i18n="btnCancel">إلغاء</span>
            </button>
            <button type="submit" class="btn btn-primary">
              <i class="fas fa-save"></i>
              <span data-i18n="btnSave">حفظ</span>
            </button>
          </div>
        </form>
      </div>
    </div>
  `;

  document.body.insertAdjacentHTML("beforeend", formHTML);

  document.getElementById("packageForm").addEventListener("submit", (e) => {
    e.preventDefault();
    const categoryIdValue =
      document.getElementById("categoryIdHidden")?.value || document.getElementById("categoryId").value;
    const formData = {
      id: document.getElementById("packageId").value,
      titleAr: document.getElementById("pkgTitleAr").value,
      titleEn: document.getElementById("pkgTitleEn").value,
      categoryId: categoryIdValue,
      shortDescriptionAr: document.getElementById("pkgShortDescriptionAr").value,
      shortDescriptionEn: document.getElementById("pkgShortDescriptionEn").value,
      rank: Number.parseInt(document.getElementById("rank").value),
      generalDiscount: Number.parseFloat(document.getElementById("generalDiscount").value),
      upgradeDiscount: Number.parseFloat(document.getElementById("upgradeDiscount").value),
      defaultDiscountRate: Number.parseFloat(document.getElementById("defaultDiscountRate").value),
      isDefault: document.getElementById("isDefault").value === "true",
      isLastPackage: document.getElementById("isLastPackage").value === "true",
      currentState: 1,
    };
    const savedPackage = savePackage(formData);

    // Save package courses
    const selectedCoursesCheckboxes = document.querySelectorAll('input[name="packageCourses"]:checked');
    const newPackageCourses = Array.from(selectedCoursesCheckboxes).map((cb, index) => ({
      id: `${savedPackage.id}-${cb.value}`,
      packageId: savedPackage.id,
      courseId: cb.value,
      displayOrder: index + 1,
    }));

    // Remove old package courses and add new ones
    let allPackageCourses = JSON.parse(localStorage.getItem("packageCourses") || "[]");
    allPackageCourses = allPackageCourses.filter((pc) => pc.packageId !== savedPackage.id);
    allPackageCourses.push(...newPackageCourses);
    localStorage.setItem("packageCourses", JSON.stringify(allPackageCourses));

    closePackageModal();
    if (currentDiplomaId) {
      renderDiplomaPackages(currentDiplomaId);
    } else {
      renderPackages(currentPackagesView);
    }
  });

  updateTranslations();
}

function closePackageModal(event) {
  if (event && event.target.classList.contains("modal-content")) return;
  const modal = document.getElementById("packageModal");
  if (modal) modal.remove();
}

function editPackage(id) {
  showPackageFormWithCourses(id);
}

function deletePackageConfirm(id) {
  if (confirm("هل أنت متأكد من حذف هذه الباقة؟")) {
    deletePackage(id);
    renderPackages(currentPackagesView);
  }
}

// Package Details Functions
function showPackageDetails(packageId) {
  currentPackageId = packageId;
  const pkg = loadPackages().find((p) => p.id === packageId);
  if (!pkg) return;

  showPage("package-details");

  // Update breadcrumb and title
  document.getElementById("packageDetailsBreadcrumb").textContent =
    currentLanguage === "ar" ? pkg.titleAr : pkg.titleEn;
  document.getElementById("packageDetailsTitle").textContent =
    currentLanguage === "ar" ? pkg.titleAr : pkg.titleEn;

  // Display package info
  const infoContent = document.getElementById("packageInfoContent");
  infoContent.innerHTML = `
    <div class="package-details-container">
      <div class="package-details-content">
        <div class="detail-row">
          <div class="detail-label">العنوان (عربي):</div>
          <div class="detail-value">${pkg.titleAr}</div>
        </div>
        <div class="detail-row">
          <div class="detail-label">العنوان (إنجليزي):</div>
          <div class="detail-value">${pkg.titleEn}</div>
        </div>
        <div class="detail-row">
          <div class="detail-label">الترتيب:</div>
          <div class="detail-value">${pkg.rank}</div>
        </div>
        <div class="detail-row">
          <div class="detail-label">الخصم العام:</div>
          <div class="detail-value">${pkg.generalDiscount}%</div>
        </div>
        <div class="detail-row">
          <div class="detail-label">خصم الترقية:</div>
          <div class="detail-value">${pkg.upgradeDiscount}%</div>
        </div>
        <div class="detail-row">
          <div class="detail-label">معدل الخصم الافتراضي:</div>
          <div class="detail-value">${pkg.defaultDiscountRate}%</div>
        </div>
        <div class="detail-row">
          <div class="detail-label">باقة افتراضية:</div>
          <div class="detail-value">${pkg.isDefault ? "نعم" : "لا"}</div>
        </div>
        <div class="detail-row">
          <div class="detail-label">آخر باقة:</div>
          <div class="detail-value">${pkg.isLastPackage ? "نعم" : "لا"}</div>
        </div>
      </div>
    </div>
  `;

  // Render package courses
  renderPackageCourses(packageId);
  
  // Render package reviews
  renderPackageReviews(packageId);
}

function renderPackageCourses(packageId) {
  const packageCourses = JSON.parse(localStorage.getItem("packageCourses") || "[]")
    .filter(pc => pc.packageId === packageId)
    .map(pc => loadCourses().find(c => c.id === pc.courseId))
    .filter(Boolean);
    
  const grid = document.getElementById("packageCoursesGrid");

  if (packageCourses.length === 0) {
    grid.innerHTML = '<p class="empty-message">لا توجد دورات في هذه الباقة</p>';
    return;
  }

  grid.innerHTML = packageCourses
    .map(
      (course) => `
    <div class="diploma-card">
      <div class="diploma-badge">
        <i class="fas fa-book"></i>
      </div>
      <div class="diploma-content">
        <div class="diploma-course">${currentLanguage === "ar" ? course.titleAr : course.titleEn}</div>
        <div class="diploma-student">${currentLanguage === "ar" ? course.shortDescriptionAr : course.shortDescriptionEn}</div>
        <div class="diploma-meta">
          <div class="diploma-date">
            <i class="fas fa-clock"></i>
            <span>${course.duration}</span>
          </div>
          <div class="diploma-grade">
            <i class="fas fa-dollar-sign"></i>
            <span>${course.isFree ? "مجاني" : course.price + " جنيه"}</span>
          </div>
        </div>
      </div>
    </div>
  `,
    )
    .join("");
}

// Package Reviews Functions
function renderPackageReviews(packageId) {
  const reviews = loadReviews().filter((r) => r.referenceId === packageId && r.reviewType === 2);
  const grid = document.getElementById("packageReviewsGrid");

  if (reviews.length === 0) {
    grid.innerHTML = '<p class="empty-message">لا توجد تقييمات لهذه الباقة</p>';
    return;
  }

  grid.innerHTML = reviews
    .map(
      (review) => `
    <div class="review-card">
      <div class="review-header">
        <img src="${review.imagePath || "/placeholder-avatar.jpg"}" alt="${review.fullName}" class="review-avatar">
        <div class="review-info">
          <div class="review-name">${review.fullName}</div>
          <div class="review-date">${new Date(review.createdDate).toLocaleDateString(currentLanguage === "ar" ? "ar-EG" : "en-US")}</div>
        </div>
      </div>
      <div class="review-rating">
        ${Array.from({ length: 5 }, (_, i) => 
          `<i class="fas fa-star${i < 4 ? "" : "-o"}"></i>`
        ).join("")}
      </div>
      <div class="review-content">${review.comment}</div>
      ${review.videoPath ? 
        `<video src="${review.videoPath}" class="review-video" controls></video>` : 
        ""
      }
      <div class="review-actions">
        <button class="btn-icon" onclick="editPackageReview('${review.id}')">
          <i class="fas fa-edit"></i>
        </button>
        <button class="btn-icon" onclick="deletePackageReviewConfirm('${review.id}')">
          <i class="fas fa-trash"></i>
        </button>
      </div>
    </div>
  `,
    )
    .join("");
}

function showPackageReviewForm(reviewId = null) {
  const review = reviewId ? loadReviews().find((r) => r.id === reviewId) : null;

  const formHTML = `
    <div class="modal-overlay" id="packageReviewModal" onclick="closePackageReviewModal(event)">
      <div class="modal-content" onclick="event.stopPropagation()">
        <div class="modal-header">
          <h2>${review ? "تعديل تقييم الباقة" : "إضافة تقييم جديد للباقة"}</h2>
          <button class="btn-icon" onclick="closePackageReviewModal()">
            <i class="fas fa-times"></i>
          </button>
        </div>
        <form id="packageReviewForm" class="modal-body">
          <input type="hidden" id="packageReviewId" value="${review?.id || ""}" />
          
          <div class="form-group">
            <label for="packageReviewFullName">الاسم</label>
            <input type="text" id="packageReviewFullName" value="${review?.fullName || ""}" required />
          </div>

          <div class="form-group">
            <label for="packageReviewComment">التعليق</label>
            <textarea id="packageReviewComment" rows="4" required>${review?.comment || ""}</textarea>
          </div>

          <div class="form-group">
            <label for="packageReviewVideoPath">رابط الفيديو (اختياري)</label>
            <input type="text" id="packageReviewVideoPath" value="${review?.videoPath || ""}" />
          </div>

          <div class="form-group">
            <label for="packageReviewImagePath">رابط الصورة (اختياري)</label>
            <input type="text" id="packageReviewImagePath" value="${review?.imagePath || ""}" />
          </div>

          <div class="form-group">
            <label for="packageReviewDisplayOrder">ترتيب العرض</label>
            <input type="number" id="packageReviewDisplayOrder" value="${review?.displayOrder || 1}" required />
          </div>

          <div class="modal-footer">
            <button type="button" class="btn" onclick="closePackageReviewModal()">
              <span>إلغاء</span>
            </button>
            <button type="submit" class="btn btn-primary">
              <i class="fas fa-save"></i>
              <span>حفظ</span>
            </button>
          </div>
        </form>
      </div>
    </div>
  `;

  document.body.insertAdjacentHTML("beforeend", formHTML);

  document.getElementById("packageReviewForm").addEventListener("submit", (e) => {
    e.preventDefault();
    const formData = {
      id: document.getElementById("packageReviewId").value,
      reviewType: 2, // Package review
      referenceId: currentPackageId,
      fullName: document.getElementById("packageReviewFullName").value,
      comment: document.getElementById("packageReviewComment").value,
      videoPath: document.getElementById("packageReviewVideoPath").value,
      imagePath: document.getElementById("packageReviewImagePath").value,
      displayOrder: Number.parseInt(document.getElementById("packageReviewDisplayOrder").value),
      currentState: 1,
    };
    savePackageReview(formData);
    closePackageReviewModal();
    renderPackageReviews(currentPackageId);
  });
}

function closePackageReviewModal(event) {
  if (event && event.target.classList.contains("modal-content")) return;
  const modal = document.getElementById("packageReviewModal");
  if (modal) modal.remove();
}

function savePackageReview(review) {
  const reviews = loadReviews();
  if (review.id) {
    const index = reviews.findIndex((r) => r.id === review.id);
    if (index !== -1) reviews[index] = { ...reviews[index], ...review };
  } else {
    review.id = Date.now().toString();
    review.createdDate = new Date().toISOString();
    review.referenceId = currentPackageId; // Link review to current package
    review.reviewType = 2; // Package review
    reviews.push(review);
  }
  localStorage.setItem("reviews", JSON.stringify(reviews));
  return review;
}

function editPackageReview(id) {
  showPackageReviewForm(id);
}

function deletePackageReviewConfirm(id) {
  if (confirm("هل أنت متأكد من حذف هذا التقييم؟")) {
    deleteReview(id);
    renderPackageReviews(currentPackageId);
  }
}

// Courses CRUD Functions
function loadCourses() {
  return JSON.parse(localStorage.getItem("courses") || "[]");
}

function saveCourse(course) {
  const courses = loadCourses();
  if (course.id) {
    const index = courses.findIndex((c) => c.id === course.id);
    if (index !== -1) courses[index] = { ...courses[index], ...course };
  } else {
    course.id = Date.now().toString();
    course.createdDate = new Date().toISOString();
    courses.push(course);
  }
  localStorage.setItem("courses", JSON.stringify(courses));
  return course;
}

function deleteCourse(id) {
  const courses = loadCourses();
  localStorage.setItem("courses", JSON.stringify(courses.filter((c) => c.id !== id)));
}

function renderCourses(view = "grid") {
  const courses = loadCourses();
  const gridView = document.getElementById("coursesGridView");
  const tableView = document.getElementById("coursesTableView");
  const tableBody = document.getElementById("coursesTableBody");

  // Update metrics
  document.getElementById("totalCourses").textContent = courses.length;
  document.getElementById("activeCourses").textContent = courses.filter((c) => c.currentState === 1).length;
  document.getElementById("freeCourses").textContent = courses.filter((c) => c.isFree).length;

  if (view === "grid") {
    gridView.style.display = "grid";
    tableView.style.display = "none";

    gridView.innerHTML = courses
      .map(
        (course) => `
      <div class="diploma-card">
        <div class="diploma-badge">
          <i class="fas fa-book"></i>
        </div>
        <div class="diploma-content">
          <div class="diploma-course">${currentLanguage === "ar" ? course.titleAr : course.titleEn}</div>
          <div class="diploma-student">${currentLanguage === "ar" ? course.shortDescriptionAr : course.shortDescriptionEn}</div>
          <div class="diploma-meta">
            <div class="diploma-date">
              <i class="fas fa-clock"></i>
              <span>${course.duration}</span>
            </div>
            <div class="diploma-grade">
              <i class="fas fa-dollar-sign"></i>
              <span>${course.isFree ? "مجاني" : course.price + " جنيه"}</span>
            </div>
          </div>
        </div>
        <div class="diploma-actions">
          <button class="btn btn-sm" onclick="editCourse('${course.id}')">
            <i class="fas fa-edit"></i>
            <span>تعديل</span>
          </button>
          <button class="btn btn-sm" onclick="deleteCourseConfirm('${course.id}')">
            <i class="fas fa-trash"></i>
            <span>حذف</span>
          </button>
        </div>
      </div>
    `,
      )
      .join("");
  } else {
    gridView.style.display = "none";
    tableView.style.display = "block";

    tableBody.innerHTML = courses
      .map(
        (course) => `
      <tr>
        <td>
          <img src="${course.image}" alt="${course.titleEn}" class="course-thumbnail" />
        </td>
        <td>${course.titleAr}</td>
        <td>${course.titleEn}</td>
        <td>${course.duration}</td>
        <td>${course.isFree ? "مجاني" : course.price + " جنيه"}</td>
        <td>
          <span class="status-badge">${course.isFree ? "نعم" : "لا"}</span>
        </td>
        <td>
          <span class="status-badge">${course.currentState === 1 ? "نشط" : "غير نشط"}</span>
        </td>
        <td>
          <div class="table-actions">
            <button class="btn-table-action" onclick="editCourse('${course.id}')">
              <i class="fas fa-edit"></i> تعديل
            </button>
            <button class="btn-table-action" onclick="deleteCourseConfirm('${course.id}')">
              <i class="fas fa-trash"></i> حذف
            </button>
          </div>
        </td>
      </tr>
    `,
      )
      .join("");
  }

  updateTranslations();
}

function showCourseForm(courseId = null) {
  const course = courseId ? loadCourses().find((c) => c.id === courseId) : null;

  const formHTML = `
    <div class="modal-overlay" id="courseModal" onclick="closeCourseModal(event)">
      <div class="modal-content" onclick="event.stopPropagation()">
        <div class="modal-header">
          <h2>${course ? "تعديل الدورة" : "إضافة دورة جديدة"}</h2>
          <button class="btn-icon" onclick="closeCourseModal()">
            <i class="fas fa-times"></i>
          </button>
        </div>
        <form id="courseForm" class="modal-body">
          <input type="hidden" id="courseId" value="${course?.id || ""}" />
          
          <div class="form-row">
            <div class="form-group">
              <label for="courseTitleAr">العنوان (عربي)</label>
              <input type="text" id="courseTitleAr" value="${course?.titleAr || ""}" required />
            </div>
            <div class="form-group">
              <label for="courseTitleEn">العنوان (إنجليزي)</label>
              <input type="text" id="courseTitleEn" value="${course?.titleEn || ""}" required />
            </div>
          </div>

          <div class="form-row">
            <div class="form-group">
              <label for="courseShortDescriptionAr">الوصف (عربي)</label>
              <textarea id="courseShortDescriptionAr" rows="3">${course?.shortDescriptionAr || ""}</textarea>
            </div>
            <div class="form-group">
              <label for="courseShortDescriptionEn">الوصف (إنجليزي)</label>
              <textarea id="courseShortDescriptionEn" rows="3">${course?.shortDescriptionEn || ""}</textarea>
            </div>
          </div>

          <div class="form-row">
            <div class="form-group">
              <label for="courseDuration">المدة</label>
              <input type="text" id="courseDuration" value="${course?.duration || ""}" placeholder="20 ساعة" required />
            </div>
            <div class="form-group">
              <label for="coursePrice">السعر (جنيه)</label>
              <input type="number" id="coursePrice" value="${course?.price || 0}" step="0.01" />
            </div>
          </div>

          <div class="form-group">
            <label for="courseIsFree">دورة مجانية</label>
            <select id="courseIsFree">
              <option value="true" ${course?.isFree ? "selected" : ""}>نعم</option>
              <option value="false" ${!course?.isFree ? "selected" : ""}>لا</option>
            </select>
          </div>

          <div class="form-group">
            <label for="courseImage">رابط الصورة</label>
            <input type="text" id="courseImage" value="${course?.image || ""}" />
          </div>

          <div class="modal-footer">
            <button type="button" class="btn" onclick="closeCourseModal()">
              <span>إلغاء</span>
            </button>
            <button type="submit" class="btn btn-primary">
              <i class="fas fa-save"></i>
              <span>حفظ</span>
            </button>
          </div>
        </form>
      </div>
    </div>
  `;

  document.body.insertAdjacentHTML("beforeend", formHTML);

  document.getElementById("courseForm").addEventListener("submit", (e) => {
    e.preventDefault();
    const formData = {
      id: document.getElementById("courseId").value,
      titleAr: document.getElementById("courseTitleAr").value,
      titleEn: document.getElementById("courseTitleEn").value,
      shortDescriptionAr: document.getElementById("courseShortDescriptionAr").value,
      shortDescriptionEn: document.getElementById("courseShortDescriptionEn").value,
      duration: document.getElementById("courseDuration").value,
      price: Number.parseFloat(document.getElementById("coursePrice").value),
      isFree: document.getElementById("courseIsFree").value === "true",
      image: document.getElementById("courseImage").value || "/placeholder-course.jpg",
      currentState: 1,
    };
    saveCourse(formData);
    closeCourseModal();
    renderCourses(currentCoursesView);
  });
}

function closeCourseModal(event) {
  if (event && event.target.classList.contains("modal-content")) return;
  const modal = document.getElementById("courseModal");
  if (modal) modal.remove();
}

function editCourse(id) {
  showCourseForm(id);
}

function deleteCourseConfirm(id) {
  if (confirm("هل أنت متأكد من حذف هذه الدورة؟")) {
    deleteCourse(id);
    renderCourses(currentCoursesView);
  }
}

// Setup Listeners Functions
function setupDiplomaDetailsListeners(diplomaId) {
  const addPackageBtn = document.getElementById("addDiplomaPackageBtn");
  if (addPackageBtn) {
    addPackageBtn.replaceWith(addPackageBtn.cloneNode(true)); // Remove old listeners
    document.getElementById("addDiplomaPackageBtn").addEventListener("click", () => {
      // Navigate to packages page instead of showing modal
      showPage("diploma-packages");
    });
  }

  const addFeatureBtn = document.getElementById("addFeatureBtn");
  if (addFeatureBtn) {
    addFeatureBtn.replaceWith(addFeatureBtn.cloneNode(true)); // Remove old listeners
    document.getElementById("addFeatureBtn").addEventListener("click", () => showFeatureForm());
  }

  const addJobBtn = document.getElementById("addJobBtn");
  if (addJobBtn) {
    addJobBtn.replaceWith(addJobBtn.cloneNode(true)); // Remove old listeners
    document.getElementById("addJobBtn").addEventListener("click", () => showJobForm());
  }

  const addProjectBtn = document.getElementById("addProjectBtn");
  if (addProjectBtn) {
    addProjectBtn.replaceWith(addProjectBtn.cloneNode(true)); // Remove old listeners
    document.getElementById("addProjectBtn").addEventListener("click", () => showProjectForm());
  }

  const addTechnologyBtn = document.getElementById("addTechnologyBtn");
  if (addTechnologyBtn) {
    addTechnologyBtn.replaceWith(addTechnologyBtn.cloneNode(true)); // Remove old listeners
    document.getElementById("addTechnologyBtn").addEventListener("click", () => showTechnologyForm());
  }
  
  const addReviewBtn = document.getElementById("addReviewBtn");
  if (addReviewBtn) {
    addReviewBtn.replaceWith(addReviewBtn.cloneNode(true)); // Remove old listeners
    document.getElementById("addReviewBtn").addEventListener("click", () => showReviewForm());
  }
}

function setupPackageDetailsPage() {
  const addPackageReviewBtn = document.getElementById("addPackageReviewBtn");
  if (addPackageReviewBtn) {
    addPackageReviewBtn.addEventListener("click", () => showPackageReviewForm());
  }
}

// UI Functions
function toggleLanguage() {
  currentLanguage = currentLanguage === "en" ? "ar" : "en";
  const html = document.documentElement;

  html.setAttribute("lang", currentLanguage);
  html.setAttribute("dir", currentLanguage === "ar" ? "rtl" : "ltr");

  updateTranslations();
  localStorage.setItem("language", currentLanguage);
}

function updateTranslations() {
  const elements = document.querySelectorAll("[data-i18n]");
  elements.forEach((element) => {
    const key = element.getAttribute("data-i18n");
    if (translations[currentLanguage][key]) {
      if (element.tagName === "INPUT") {
        element.placeholder = translations[currentLanguage][key];
      } else {
        element.innerHTML = translations[currentLanguage][key];
      }
    }
  });

  const placeholderElements = document.querySelectorAll("[data-i18n-placeholder]");
  placeholderElements.forEach((element) => {
    const key = element.getAttribute("data-i18n-placeholder");
    if (translations[currentLanguage][key]) {
      element.placeholder = translations[currentLanguage][key];
    }
  });
}

function initializeLanguage() {
  const savedLanguage = localStorage.getItem("language") || "ar";
  if (savedLanguage !== currentLanguage) {
    currentLanguage = savedLanguage;
  }
  const html = document.documentElement;
  html.setAttribute("lang", currentLanguage);
  html.setAttribute("dir", currentLanguage === "ar" ? "rtl" : "ltr");
  updateTranslations();
}

function initializeTheme() {
  const savedTheme = localStorage.getItem("theme");
  const prefersDark = window.matchMedia("(prefers-color-scheme: dark)").matches;

  if (savedTheme === "dark" || (!savedTheme && prefersDark)) {
    document.body.classList.add("dark");
    const icon = themeToggle?.querySelector("i");
    if (icon) {
      icon.classList.remove("fa-moon");
      icon.classList.add("fa-sun");
    }
    const loginIcon = loginThemeToggle?.querySelector("i");
    if (loginIcon) {
      loginIcon.classList.remove("fa-moon");
      loginIcon.classList.add("fa-sun");
    }
  }
}

// Initialize Chart
const enrollmentChart = document.getElementById("enrollmentChart");
if (enrollmentChart) {
  const ctx = enrollmentChart.getContext("2d");

  enrollmentChart.width = enrollmentChart.offsetWidth;
  enrollmentChart.height = 200;

  const gradient = ctx.createLinearGradient(0, 0, 0, 200);
  gradient.addColorStop(0, "rgba(228, 45, 35, 0.3)");
  gradient.addColorStop(1, "rgba(228, 45, 35, 0)");

  ctx.fillStyle = gradient;
  ctx.fillRect(0, 0, enrollmentChart.width, enrollmentChart.height);

  ctx.beginPath();
  ctx.moveTo(50, 150);
  ctx.lineTo(150, 120);
  ctx.lineTo(250, 90);
  ctx.lineTo(350, 60);
  ctx.lineTo(450, 40);
  ctx.lineTo(550, 20);
  ctx.strokeStyle = "hsl(228, 45%, 35%)";
  ctx.lineWidth = 3;
  ctx.stroke();

  const points = [
    { x: 50, y: 150 },
    { x: 150, y: 120 },
    { x: 250, y: 90 },
    { x: 350, y: 60 },
    { x: 450, y: 40 },
    { x: 550, y: 20 },
  ];

  points.forEach((point) => {
    ctx.beginPath();
    ctx.arc(point.x, point.y, 5, 0, 2 * Math.PI);
    ctx.fillStyle = "hsl(228, 45%, 35%)";
    ctx.fill();
  });
}

// Toggle Mobile Sidebar
menuToggle?.addEventListener("click", () => {
  sidebar.classList.toggle("open");
  overlay.classList.toggle("active");
});

overlay?.addEventListener("click", () => {
  sidebar.classList.remove("open");
  overlay.classList.remove("active");
});

// Toggle Theme
themeToggle?.addEventListener("click", () => {
  document.body.classList.toggle("dark");
  const icon = themeToggle.querySelector("i");

  if (document.body.classList.contains("dark")) {
    icon.classList.remove("fa-moon");
    icon.classList.add("fa-sun");
    localStorage.setItem("theme", "dark");
  } else {
    icon.classList.remove("fa-sun");
    icon.classList.add("fa-moon");
    localStorage.setItem("theme", "light");
  }
});

languageToggle?.addEventListener("click", toggleLanguage);

loginThemeToggle?.addEventListener("click", () => {
  document.body.classList.toggle("dark");
  const icon = loginThemeToggle.querySelector("i");

  if (document.body.classList.contains("dark")) {
    icon.classList.remove("fa-moon");
    icon.classList.add("fa-sun");
    localStorage.setItem("theme", "dark");
  } else {
    icon.classList.remove("fa-sun");
    icon.classList.add("fa-moon");
    localStorage.setItem("theme", "light");
  }
});

loginLanguageToggle?.addEventListener("click", toggleLanguage);

loginForm?.addEventListener("submit", (e) => {
  e.preventDefault();
  const email = document.getElementById("email").value;
  const password = document.getElementById("password").value;

  if (email && password) {
    localStorage.setItem("isLoggedIn", "true");
    loginPage.classList.remove("active");
    showPage("dashboard");
  }
});

function toggleDropdown(menu) {
  document.querySelectorAll(".dropdown-menu").forEach((m) => {
    if (m !== menu) {
      m.classList.remove("show");
    }
  });
  menu.classList.toggle("show");
}

notificationBtn?.addEventListener("click", (e) => {
  e.stopPropagation();
  toggleDropdown(notificationMenu);
});

userBtn?.addEventListener("click", (e) => {
  e.stopPropagation();
  toggleDropdown(userMenu);
});

logoutBtn?.addEventListener("click", (e) => {
  e.preventDefault();
  e.stopPropagation();

  localStorage.removeItem("isLoggedIn");
  loginPage.classList.add("active");

  pages.forEach((page) => {
    page.classList.remove("active");
  });

  userMenu.classList.remove("show");

  navItems.forEach((item) => {
    item.classList.remove("active");
    if (item.getAttribute("data-page") === "dashboard") {
      item.classList.add("active");
    }
  });
});

document.addEventListener("click", () => {
  document.querySelectorAll(".dropdown-menu").forEach((menu) => {
    menu.classList.remove("show");
  });
});

document.querySelectorAll(".dropdown-menu").forEach((menu) => {
  menu.addEventListener("click", (e) => {
    e.stopPropagation();
  });
});

// Page Navigation
async function showPage(pageId) {
  pages.forEach((page) => {
    page.classList.remove("active");
  });

  const selectedPage = document.getElementById(`${pageId}-page`);
  if (selectedPage) {
    if (pageId === "diplomas") {
      const addBtn = document.getElementById("addDiplomaBtn");
      if (addBtn) {
        addBtn.replaceWith(addBtn.cloneNode(true)); // Remove old listeners
        document.getElementById("addDiplomaBtn").addEventListener("click", () => showDiplomaForm());
      }

      document.querySelectorAll("#diplomas-page .view-btn").forEach((btn) => {
        btn.replaceWith(btn.cloneNode(true)); // Remove old listeners
      });

      document.querySelectorAll("#diplomas-page .view-btn").forEach((btn) => {
        btn.addEventListener("click", () => {
          document.querySelectorAll("#diplomas-page .view-btn").forEach((b) => b.classList.remove("active"));
          btn.classList.add("active");
          currentDiplomasView = btn.getAttribute("data-view");
          renderDiplomas(currentDiplomasView);
        });
      });

      renderDiplomas();
    } else if (pageId === "diploma-packages") {
      // Load the packages page content if not already loaded
      if (!document.getElementById("diploma-packages-page").innerHTML.trim()) {
        loadPackagesPage();
      } else {
        setupPackagesPage();
      }
    } else if (pageId === "package-details") {
      setupPackageDetailsPage();
    } else if (pageId === "packages") {
      const addBtn = document.getElementById("addPackageBtn");
      if (addBtn) {
        addBtn.replaceWith(addBtn.cloneNode(true)); // Remove old listeners
        document.getElementById("addPackageBtn").addEventListener("click", () => showPackageFormWithCourses());
      }

      document.querySelectorAll("#packages-page .view-btn").forEach((btn) => {
        btn.replaceWith(btn.cloneNode(true)); // Remove old listeners
      });

      document.querySelectorAll("#packages-page .view-btn").forEach((btn) => {
        btn.addEventListener("click", () => {
          document.querySelectorAll("#packages-page .view-btn").forEach((b) => b.classList.remove("active"));
          btn.classList.add("active");
          currentPackagesView = btn.getAttribute("data-view");
          renderPackages(currentPackagesView);
        });
      });

      renderPackages();
    } else if (pageId === "courses") {
      const addBtn = document.getElementById("addCourseBtn");
      if (addBtn) {
        addBtn.replaceWith(addBtn.cloneNode(true));
        document.getElementById("addCourseBtn").addEventListener("click", () => showCourseForm());
      }

      document.querySelectorAll("#courses-page .view-btn").forEach((btn) => {
        btn.replaceWith(btn.cloneNode(true));
      });

      document.querySelectorAll("#courses-page .view-btn").forEach((btn) => {
        btn.addEventListener("click", () => {
          document.querySelectorAll("#courses-page .view-btn").forEach((b) => b.classList.remove("active"));
          btn.classList.add("active");
          currentCoursesView = btn.getAttribute("data-view");
          renderCourses(currentCoursesView);
        });
      });

      renderCourses();
    } else if (pageId === "diploma-details") {
      setupDiplomaDetailsListeners(currentDiplomaId);
    }

    selectedPage.classList.add("active");
  }

  navItems.forEach((item) => {
    item.classList.remove("active");
    if (item.getAttribute("data-page") === pageId) {
      item.classList.add("active");
    }
  });

  sidebar.classList.remove("open");
  overlay.classList.remove("active");

  updateTranslations();
}

// Function to load the packages page content
function loadPackagesPage() {
  fetch('diploma-packages.html')
    .then(response => response.text())
    .then(html => {
      document.getElementById('diploma-packages-page').innerHTML = html;
      setupPackagesPage();
    })
    .catch(error => console.error('Error loading packages page:', error));
}

// Function to setup the packages page
function setupPackagesPage() {
  // Update breadcrumb and title based on current diploma
  const diploma = loadDiplomas().find((d) => d.id === currentDiplomaId);
  if (diploma) {
    document.getElementById("packagesBreadcrumb").textContent = 
      currentLanguage === "ar" ? `باقات ${diploma.titleAr}` : `${diploma.titleEn} Packages`;
    document.getElementById("packagesTitle").textContent = 
      currentLanguage === "ar" ? `باقات ${diploma.titleAr}` : `${diploma.titleEn} Packages`;
    document.getElementById("packagesDescription").textContent = 
      currentLanguage === "ar" ? `إدارة وعرض جميع باقات دبلوم ${diploma.titleAr}` : `Manage and view all packages for ${diploma.titleEn} diploma`;
  }

  // Setup event listeners
  const addPackageBtn = document.getElementById("addPackageBtn");
  if (addPackageBtn) {
    addPackageBtn.addEventListener("click", () => showPackageFormWithCourses(null, currentDiplomaId));
  }

  document.querySelectorAll("#diploma-packages-page .view-btn").forEach((btn) => {
    btn.addEventListener("click", () => {
      document.querySelectorAll("#diploma-packages-page .view-btn").forEach((b) => b.classList.remove("active"));
      btn.classList.add("active");
      currentPackagesView = btn.getAttribute("data-view");
      renderPackages(currentPackagesView);
    });
  });

  // Render packages
  renderPackages();
}

pageLinks.forEach((link) => {
  link.addEventListener("click", (e) => {
    e.preventDefault();
    const pageId = link.getAttribute("data-page");
    showPage(pageId);
  });
});

navItems.forEach((item) => {
  item.addEventListener("click", () => {
    const pageId = item.getAttribute("data-page");
    showPage(pageId);
  });
});

document.querySelectorAll(".quick-action-btn").forEach((btn) => {
  btn.addEventListener("click", (e) => {
    e.preventDefault();
    const pageId = btn.getAttribute("data-page");
    if (pageId) {
      showPage(pageId);
    }
  });
});

// Initialize Application
function init() {
  initializeLanguage();
  initializeTheme();
  initializeSampleData();

  const isLoggedIn = localStorage.getItem("isLoggedIn");

  if (isLoggedIn === "true") {
    loginPage.classList.remove("active");
    showPage("dashboard");
  } else {
    loginPage.classList.add("active");
    pages.forEach((page) => {
      page.classList.remove("active");
    });
  }
}

// Start the application
init();
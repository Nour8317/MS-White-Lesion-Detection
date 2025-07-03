// DOM Elements
const navbar = document.getElementById('navbar');
const navLinks = document.getElementById('nav-links');
const hamburger = document.getElementById('hamburger');
const pages = document.querySelectorAll('.page');
const navLinkElements = document.querySelectorAll('.nav-link');
const tabButtons = document.querySelectorAll('.tab-btn');
const tabContents = document.querySelectorAll('.tab-content');

// Chart instance
let resultsChart = null;

// Initialize the application
document.addEventListener('DOMContentLoaded', function () {
    initializeNavigation();
    initializeForms();
    initializeUploadArea();
    initializeTabs();
    initializeChart();
    showPage('home'); // Set Home as default page
});

// Navigation functionality
function initializeNavigation() {
    // Mobile menu toggle
    hamburger.addEventListener('click', function () {
        navLinks.classList.toggle('active');
        hamburger.classList.toggle('active');
    });

    // Navigation links
    navLinkElements.forEach(link => {
        link.addEventListener('click', function (e) {
            e.preventDefault();
            const targetPage = this.getAttribute('data-page');
            showPage(targetPage);

            // Close mobile menu
            navLinks.classList.remove('active');
            hamburger.classList.remove('active');
        });
    });

    // Form links (for switching between signup/signin)
    document.querySelectorAll('[data-page]').forEach(link => {
        link.addEventListener('click', function (e) {
            e.preventDefault();
            const targetPage = this.getAttribute('data-page');
            showPage(targetPage);
        });
    });
}

// Show specific page
function showPage(pageName) {
    // Hide all pages
    pages.forEach(page => {
        page.classList.remove('active');
    });

    // Show target page
    const targetPage = document.getElementById(`${pageName}-page`);
    if (targetPage) {
        targetPage.classList.add('active');

        // Update active nav link
        navLinkElements.forEach(link => {
            link.classList.remove('active');
            if (link.getAttribute('data-page') === pageName) {
                link.classList.add('active');
            }
        });
    }

    
    
}

// Form handling
function initializeForms() {
    // Sign Up Form
    const signupForm = document.getElementById('signup-form');
    if (signupForm) {
        signupForm.addEventListener('submit', function (e) {
            e.preventDefault();
            handleSignup();
        });
    }

    // Sign In Form
    const signinForm = document.getElementById('signin-form');
    if (signinForm) {
        signinForm.addEventListener('submit', function (e) {
            e.preventDefault();
            handleSignin();
        });
    }

    // Profile Form
    const profileForm = document.getElementById('profile-form');
    if (profileForm) {
        profileForm.addEventListener('submit', function (e) {
            e.preventDefault();
            handleProfileUpdate();
        });
    }

    // Form validation
    initializeFormValidation();
}

// Form validation
function initializeFormValidation() {
    // Password confirmation validation
    const signupPassword = document.getElementById('signup-password');
    const signupConfirmPassword = document.getElementById('signup-confirm-password');

    if (signupPassword && signupConfirmPassword) {
        signupConfirmPassword.addEventListener('input', function () {
            validatePasswordMatch();
        });
    }

    // Email validation
    const emailInputs = document.querySelectorAll('input[type="email"]');
    emailInputs.forEach(input => {
        input.addEventListener('blur', function () {
            validateEmail(this);
        });
    });

    // Phone validation
    const phoneInputs = document.querySelectorAll('input[type="tel"]');
    phoneInputs.forEach(input => {
        input.addEventListener('blur', function () {
            validatePhone(this);
        });
    });
}

// Validate password match
function validatePasswordMatch() {
    const password = document.getElementById('signup-password');
    const confirmPassword = document.getElementById('signup-confirm-password');

    if (password && confirmPassword) {
        const formGroup = confirmPassword.closest('.form-group');

        if (confirmPassword.value && password.value !== confirmPassword.value) {
            showFieldError(formGroup, 'Passwords do not match');
        } else {
            clearFieldError(formGroup);
        }
    }
}

// Validate email
function validateEmail(input) {
    const email = input.value;
    const formGroup = input.closest('.form-group');
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if (email && !emailRegex.test(email)) {
        showFieldError(formGroup, 'Please enter a valid email address');
    } else {
        clearFieldError(formGroup);
    }
}

// Validate phone
function validatePhone(input) {
    const phone = input.value;
    const formGroup = input.closest('.form-group');
    const phoneRegex = /^[\+]?[1-9][\d]{0,15}$/;

    if (phone && !phoneRegex.test(phone.replace(/\s/g, ''))) {
        showFieldError(formGroup, 'Please enter a valid phone number');
    } else {
        clearFieldError(formGroup);
    }
}

// Show field error
function showFieldError(formGroup, message) {
    clearFieldError(formGroup);

    formGroup.classList.add('error');
    const errorDiv = document.createElement('div');
    errorDiv.className = 'error-message';
    errorDiv.textContent = message;
    formGroup.appendChild(errorDiv);
}

// Clear field error
function clearFieldError(formGroup) {
    formGroup.classList.remove('error');
    const errorMessage = formGroup.querySelector('.error-message');
    if (errorMessage) {
        errorMessage.remove();
    }
}

// Handle signup
function handleSignup() {
    const form = document.getElementById('signup-form');
    const formData = new FormData(form);

    // Basic validation
    const email = document.getElementById('signup-email').value;
    const username = document.getElementById('signup-username').value;
    const phone = document.getElementById('signup-phone').value;
    const password = document.getElementById('signup-password').value;
    const confirmPassword = document.getElementById('signup-confirm-password').value;

    if (!email || !username || !phone || !password || !confirmPassword) {
        showMessage('Please fill in all fields', 'error');
        return;
    }

    if (password !== confirmPassword) {
        showMessage('Passwords do not match', 'error');
        return;
    }

    // Simulate signup process
    showMessage('Registration successful!', 'success');
    setTimeout(() => {
        showPage('signin');
    }, 2000);
}

// Handle signin
function handleSignin() {
    const email = document.getElementById('signin-email').value;
    const password = document.getElementById('signin-password').value;

    if (!email || !password) {
        showMessage('Please fill in all fields', 'error');
        return;
    }

    // Simulate signin process
    showMessage('Login successful!', 'success');
    setTimeout(() => {
        showPage('upload');
    }, 2000);
}

// Handle profile update
function handleProfileUpdate() {
    const name = document.getElementById('profile-name').value;
    const email = document.getElementById('profile-email').value;

    if (!name || !email) {
        showMessage('Please fill in required fields', 'error');
        return;
    }

    showMessage('Profile updated successfully!', 'success');
}

// Upload area functionality
function initializeUploadArea() {
    const uploadArea = document.getElementById('upload-area');
    const fileInput = document.getElementById('file-input');
    const browseBtn = document.getElementById('browse-btn');

    if (uploadArea && fileInput && browseBtn) {
        // Browse button
        browseBtn.addEventListener('click', function () {
            fileInput.click();
        });

        // File input change
        fileInput.addEventListener('change', function () {
            handleFileSelection(this.files);
        });

        // Drag and drop
        uploadArea.addEventListener('dragover', function (e) {
            e.preventDefault();
            this.classList.add('dragover');
        });

        uploadArea.addEventListener('dragleave', function (e) {
            e.preventDefault();
            this.classList.remove('dragover');
        });

        uploadArea.addEventListener('drop', function (e) {
            e.preventDefault();
            this.classList.remove('dragover');
            const files = e.dataTransfer.files;
            handleFileSelection(files);
        });
    }

    // Profile picture upload
    const profilePictureInput = document.getElementById('profile-picture');
    const uploadPhotoBtn = document.getElementById('upload-photo-btn');

    if (profilePictureInput && uploadPhotoBtn) {
        uploadPhotoBtn.addEventListener('click', function () {
            profilePictureInput.click();
        });

        profilePictureInput.addEventListener('change', function () {
            handleProfilePictureUpload(this.files[0]);
        });
    }
}

// Handle file selection
function handleFileSelection(files) {
    if (files.length === 0) return;

    const uploadArea = document.getElementById('upload-area');
    const supportedFormats = ['jpg', 'jpeg', 'png', 'svg', 'zip'];

    for (let file of files) {
        const fileExtension = file.name.split('.').pop().toLowerCase();

        if (!supportedFormats.includes(fileExtension)) {
            showMessage(`File type .${fileExtension} is not supported`, 'error');
            continue;
        }

        if (file.size > 10 * 1024 * 1024) { // 10MB limit
            showMessage(`File ${file.name} is too large. Maximum size is 10MB`, 'error');
            continue;
        }

        // Show file info
        const fileInfo = document.createElement('div');
        fileInfo.className = 'file-info';
        fileInfo.innerHTML = `
            <span>${file.name}</span>
            <span>${formatFileSize(file.size)}</span>
        `;

        uploadArea.appendChild(fileInfo);
    }

    showMessage(`${files.length} file(s) selected successfully`, 'success');
}

// Handle profile picture upload
function handleProfilePictureUpload(file) {
    if (!file) return;

    const supportedFormats = ['jpg', 'jpeg', 'png', 'gif'];
    const fileExtension = file.name.split('.').pop().toLowerCase();

    if (!supportedFormats.includes(fileExtension)) {
        showMessage('Please select a valid image file (JPG, PNG, GIF)', 'error');
        return;
    }

    if (file.size > 5 * 1024 * 1024) { // 5MB limit
        showMessage('Image file is too large. Maximum size is 5MB', 'error');
        return;
    }

    showMessage('Profile picture uploaded successfully!', 'success');
}

// Format file size
function formatFileSize(bytes) {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
}

// Tab functionality
function initializeTabs() {
    tabButtons.forEach(button => {
        button.addEventListener('click', function () {
            const tabName = this.getAttribute('data-tab');
            switchTab(tabName);
        });
    });
}

// Switch tab
function switchTab(tabName) {
    // Update tab buttons
    tabButtons.forEach(btn => {
        btn.classList.remove('active');
        if (btn.getAttribute('data-tab') === tabName) {
            btn.classList.add('active');
        }
    });

    // Update tab content
    tabContents.forEach(content => {
        content.classList.remove('active');
        if (content.id === `${tabName}-tab`) {
            content.classList.add('active');
        }
    });
}

// Initialize chart
function initializeChart() {
    const chartCanvas = document.getElementById('results-chart');
    if (chartCanvas) {
        const ctx = chartCanvas.getContext('2d');

        resultsChart = new Chart(ctx, {
            type: 'line',
            data: {
                labels: ['JAN', 'FEB', 'MAR', 'APR', 'MAY', 'JUN', 'JUL', 'AUG', 'SEP', 'OCT', 'NOV', 'DEC'],
                datasets: [
                    {
                        label: 'Lorem',
                        data: [65, 59, 80, 81, 56, 55, 40, 45, 60, 70, 75, 80],
                        borderColor: '#1E3A8A',
                        backgroundColor: 'rgba(30, 58, 138, 0.1)',
                        tension: 0.4,
                        fill: true
                    },
                    {
                        label: 'Ipsum',
                        data: [28, 48, 40, 19, 86, 27, 90, 85, 70, 60, 65, 70],
                        borderColor: '#9333EA',
                        backgroundColor: 'rgba(147, 51, 234, 0.1)',
                        tension: 0.4,
                        fill: true
                    }
                ]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        position: 'top',
                    },
                    title: {
                        display: true,
                        text: 'Monthly Results'
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        max: 100
                    }
                }
            }
        });
    }
}

// Show message
function showMessage(message, type = 'info') {
    // Remove existing messages
    const existingMessages = document.querySelectorAll('.message');
    existingMessages.forEach(msg => msg.remove());

    // Create message element
    const messageDiv = document.createElement('div');
    messageDiv.className = `message ${type}`;
    messageDiv.textContent = message;

    // Style the message
    messageDiv.style.cssText = `
        position: fixed;
        top: 90px;
        right: 20px;
        padding: 15px 20px;
        border-radius: 8px;
        color: white;
        font-weight: 500;
        z-index: 1001;
        animation: slideIn 0.3s ease-out;
        max-width: 300px;
    `;

    // Set background color based on type
    switch (type) {
        case 'success':
            messageDiv.style.backgroundColor = '#10B981';
            break;
        case 'error':
            messageDiv.style.backgroundColor = '#EF4444';
            break;
        default:
            messageDiv.style.backgroundColor = '#3B82F6';
    }

    // Add to page
    document.body.appendChild(messageDiv);

    // Remove after 5 seconds
    setTimeout(() => {
        messageDiv.style.animation = 'slideOut 0.3s ease-in';
        setTimeout(() => {
            if (messageDiv.parentNode) {
                messageDiv.remove();
            }
        }, 300);
    }, 5000);
}

// Add CSS animations for messages
const style = document.createElement('style');
style.textContent = `
    @keyframes slideIn {
        from {
            transform: translateX(100%);
            opacity: 0;
        }
        to {
            transform: translateX(0);
            opacity: 1;
        }
    }
    
    @keyframes slideOut {
        from {
            transform: translateX(0);
            opacity: 1;
        }
        to {
            transform: translateX(100%);
            opacity: 0;
        }
    }
`;
document.head.appendChild(style);

// Button event handlers
document.addEventListener('click', function (e) {
    // Cancel buttons
    if (e.target.classList.contains('btn-cancel')) {
        if (e.target.closest('#upload-page')) {
            showPage('signin');
        } else if (e.target.closest('#results-page')) {
            showPage('upload');
        } else if (e.target.closest('#profile-page')) {
            // Reset form
            const form = e.target.closest('form');
            if (form) form.reset();
        }
    }

    // Next button in upload page
    if (e.target.textContent === 'Next' && e.target.closest('#upload-page')) {
        showPage('results');
    }

    // Save button in results page
    if (e.target.textContent === 'Save' && e.target.closest('#results-page')) {
        showMessage('Results saved successfully!', 'success');
    }
});

// Window resize handler for responsive design
window.addEventListener('resize', function () {
    if (window.innerWidth > 768) {
        navLinks.classList.remove('active');
        hamburger.classList.remove('active');
    }
});

// Prevent form submission on Enter key for non-submit buttons
document.addEventListener('keydown', function (e) {
    if (e.key === 'Enter' && e.target.classList.contains('btn') && e.target.type !== 'submit') {
        e.preventDefault();
    }
}); 
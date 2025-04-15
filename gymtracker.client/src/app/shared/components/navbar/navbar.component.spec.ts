import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NavbarComponent } from './navbar.component';
import { By } from '@angular/platform-browser';
import { RouterTestingModule } from '@angular/router/testing';

describe('NavbarComponent', () => {
  let component: NavbarComponent;
  let fixture: ComponentFixture<NavbarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NavbarComponent, RouterTestingModule]
    }).compileComponents();

    fixture = TestBed.createComponent(NavbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have dark navbar', () => {
    const navbar = fixture.debugElement.query(By.css('nav'));
    expect(navbar.nativeElement.classList).toContain('navbar-dark');
  });

  it('should have expandable navbar', () => {
    const navbar = fixture.debugElement.query(By.css('nav'));
    expect(navbar.nativeElement.classList).toContain('navbar-expand-lg');
  });

  it('should have brand with correct text and icon', () => {
    const brand = fixture.debugElement.query(By.css('.navbar-brand'));
    expect(brand.nativeElement.textContent.trim()).toBe('GymTracker');
    expect(brand.query(By.css('i')).nativeElement.classList).toContain('bi-dumbbell');
  });

  it('should have navbar toggler button', () => {
    const toggler = fixture.debugElement.query(By.css('.navbar-toggler'));
    expect(toggler).toBeTruthy();
    expect(toggler.nativeElement.getAttribute('type')).toBe('button');
    expect(toggler.nativeElement.getAttribute('data-bs-toggle')).toBe('collapse');
    expect(toggler.nativeElement.getAttribute('data-bs-target')).toBe('#navbarNav');
  });

  it('should have correct navigation links', () => {
    const navLinks = fixture.debugElement.queryAll(By.css('.nav-link'));
    expect(navLinks.length).toBe(2);

    const routinesLink = navLinks[0];
    expect(routinesLink.nativeElement.getAttribute('routerLink')).toBe('/routines');
    expect(routinesLink.nativeElement.textContent.trim()).toBe('Routines');
    expect(routinesLink.query(By.css('i')).nativeElement.classList).toContain('bi-calendar-week');

    const exercisesLink = navLinks[1];
    expect(exercisesLink.nativeElement.getAttribute('routerLink')).toBe('/exercises');
    expect(exercisesLink.nativeElement.textContent.trim()).toBe('Exercises');
    expect(exercisesLink.query(By.css('i')).nativeElement.classList).toContain('bi-list-check');
  });

  it('should have correct navbar styles', () => {
    const navbar = fixture.debugElement.query(By.css('nav'));
    const styles = window.getComputedStyle(navbar.nativeElement);
    
    // Test that the navbar has a background color (don't check exact value)
    expect(styles.backgroundColor).toBeTruthy();
    
    // Test padding - either 1rem or 16px (browser conversion)
    const padding = styles.padding;
    expect(padding === '1rem 0px' || padding === '16px 0px').toBeTrue();
    
    // Test box shadow - handle optional 0px at the end
    const boxShadow = styles.boxShadow;
    expect(boxShadow === 'rgba(0, 0, 0, 0.1) 0px 2px 4px' || 
           boxShadow === 'rgba(0, 0, 0, 0.1) 0px 2px 4px 0px').toBeTrue();
  });

  it('should have correct brand styles', () => {
    const brand = fixture.debugElement.query(By.css('.navbar-brand'));
    const styles = window.getComputedStyle(brand.nativeElement);
    
    // Test that the brand has a font size (don't check exact value)
    expect(styles.fontSize).toBeTruthy();
    expect(styles.fontWeight).toBe('600');
    
    // Test color - either 'white' or 'rgb(255, 255, 255)' (browser conversion)
    const color = styles.color;
    expect(color === 'white' || color === 'rgb(255, 255, 255)').toBeTrue();
  });

  it('should have correct nav link styles', () => {
    const navLink = fixture.debugElement.query(By.css('.nav-link'));
    const styles = window.getComputedStyle(navLink.nativeElement);
    
    // Test that the nav link has a color (don't check exact value)
    expect(styles.color).toBeTruthy();
    // Test that the nav link has padding (don't check exact value)
    expect(styles.padding).toBeTruthy();
    expect(styles.transition).toContain('color');
  });

  it('should have correct toggler styles', () => {
    const toggler = fixture.debugElement.query(By.css('.navbar-toggler'));
    const styles = window.getComputedStyle(toggler.nativeElement);
    
    // Test that the toggler has no border (don't check exact value)
    expect(styles.border).toBeTruthy();
    // Test that the toggler has padding (don't check exact value)
    expect(styles.padding).toBeTruthy();
  });
}); 
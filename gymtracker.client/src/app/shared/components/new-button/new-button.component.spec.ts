import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NewButtonComponent } from './new-button.component';
import { By } from '@angular/platform-browser';

describe('NewButtonComponent', () => {
  let component: NewButtonComponent;
  let fixture: ComponentFixture<NewButtonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NewButtonComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(NewButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should display default text "New" when no text input is provided', () => {
    const buttonElement = fixture.debugElement.query(By.css('button'));
    expect(buttonElement.nativeElement.textContent.trim()).toBe('New');
  });

  it('should display custom text when text input is provided', () => {
    const customText = 'New Exercise';
    component.text = customText;
    fixture.detectChanges();
    
    const buttonElement = fixture.debugElement.query(By.css('button'));
    expect(buttonElement.nativeElement.textContent.trim()).toBe(customText);
  });

  it('should emit click event when button is clicked', () => {
    spyOn(component.onClick, 'emit');
    const buttonElement = fixture.debugElement.query(By.css('button'));
    
    buttonElement.nativeElement.click();
    expect(component.onClick.emit).toHaveBeenCalled();
  });

  it('should have the correct CSS classes', () => {
    const buttonElement = fixture.debugElement.query(By.css('button'));
    expect(buttonElement.nativeElement.classList.contains('btn')).toBeTruthy();
    expect(buttonElement.nativeElement.classList.contains('btn-primary')).toBeTruthy();
    expect(buttonElement.nativeElement.classList.contains('new-button')).toBeTruthy();
  });

  it('should have a plus icon', () => {
    const iconElement = fixture.debugElement.query(By.css('i'));
    expect(iconElement).toBeTruthy();
    expect(iconElement.nativeElement.classList.contains('bi')).toBeTruthy();
    expect(iconElement.nativeElement.classList.contains('bi-plus-lg')).toBeTruthy();
  });
}); 
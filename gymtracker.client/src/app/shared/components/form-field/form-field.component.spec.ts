import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormFieldComponent } from './form-field.component';
import { By } from '@angular/platform-browser';

describe('FormFieldComponent', () => {
  let component: FormFieldComponent;
  let fixture: ComponentFixture<FormFieldComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FormFieldComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(FormFieldComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should display label text', () => {
    component.label = 'Test Label';
    component.id = 'test-id';
    fixture.detectChanges();

    const labelElement = fixture.debugElement.query(By.css('label'));
    expect(labelElement.nativeElement.textContent.trim()).toBe('Test Label');
    expect(labelElement.nativeElement.getAttribute('for')).toBe('test-id');
  });

  it('should have bold label', () => {
    const labelElement = fixture.debugElement.query(By.css('label'));
    expect(labelElement.nativeElement.classList).toContain('fw-bold');
  });

  it('should display icon in input group', () => {
    component.icon = 'bi-person';
    fixture.detectChanges();

    const iconElement = fixture.debugElement.query(By.css('.input-group-text i'));
    expect(iconElement.nativeElement.classList).toContain('bi-person');
  });

  it('should have bottom margin', () => {
    const container = fixture.debugElement.query(By.css('div'));
    expect(container.nativeElement.classList).toContain('mb-4');
  });

  it('should have input group structure', () => {
    const inputGroup = fixture.debugElement.query(By.css('.input-group'));
    expect(inputGroup).toBeTruthy();
    
    const inputGroupText = fixture.debugElement.query(By.css('.input-group-text'));
    expect(inputGroupText).toBeTruthy();
  });

  it('should project content into main slot', () => {
    const testContent = '<input type="text" class="form-control">';
    fixture = TestBed.createComponent(FormFieldComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    
    const container = fixture.debugElement.query(By.css('div'));
    container.nativeElement.innerHTML = testContent;
    fixture.detectChanges();
    
    const input = fixture.debugElement.query(By.css('input'));
    expect(input).toBeTruthy();
  });

  it('should project error content into error slot', () => {
    const testError = '<div class="invalid-feedback">Error message</div>';
    fixture = TestBed.createComponent(FormFieldComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    
    const container = fixture.debugElement.query(By.css('div'));
    container.nativeElement.innerHTML = testError;
    fixture.detectChanges();
    
    const error = fixture.debugElement.query(By.css('.invalid-feedback'));
    expect(error).toBeTruthy();
  });
}); 
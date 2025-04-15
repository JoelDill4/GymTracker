import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ModalFooterComponent } from './modal-footer.component';
import { By } from '@angular/platform-browser';

describe('ModalFooterComponent', () => {
  let component: ModalFooterComponent;
  let fixture: ComponentFixture<ModalFooterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ModalFooterComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(ModalFooterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should display "Create" button text when not editing', () => {
    component.isEditing = false;
    fixture.detectChanges();

    const submitButton = fixture.debugElement.query(By.css('.btn-primary'));
    expect(submitButton.nativeElement.textContent.trim()).toBe('Create');
  });

  it('should display "Save Changes" button text when editing', () => {
    component.isEditing = true;
    fixture.detectChanges();

    const submitButton = fixture.debugElement.query(By.css('.btn-primary'));
    expect(submitButton.nativeElement.textContent.trim()).toBe('Save Changes');
  });

  it('should have correct icon for create mode', () => {
    component.isEditing = false;
    fixture.detectChanges();

    const submitButton = fixture.debugElement.query(By.css('.btn-primary i'));
    expect(submitButton.nativeElement.classList).toContain('bi-check-circle');
  });

  it('should have correct icon for edit mode', () => {
    component.isEditing = true;
    fixture.detectChanges();

    const submitButton = fixture.debugElement.query(By.css('.btn-primary i'));
    expect(submitButton.nativeElement.classList).toContain('bi-save');
  });

  it('should emit cancel event when cancel button is clicked', () => {
    spyOn(component.cancel, 'emit');
    const cancelButton = fixture.debugElement.query(By.css('.btn-outline-secondary'));
    cancelButton.triggerEventHandler('click', null);
    
    expect(component.cancel.emit).toHaveBeenCalled();
  });

  it('should have cancel button with correct icon', () => {
    const cancelButton = fixture.debugElement.query(By.css('.btn-outline-secondary i'));
    expect(cancelButton.nativeElement.classList).toContain('bi-x-circle');
  });

  it('should disable submit button when disabled input is true', () => {
    component.disabled = true;
    fixture.detectChanges();

    const submitButton = fixture.debugElement.query(By.css('.btn-primary'));
    expect(submitButton.nativeElement.disabled).toBeTrue();
  });

  it('should have correct submit type', () => {
    component.submitType = 'button';
    fixture.detectChanges();

    const submitButton = fixture.debugElement.query(By.css('.btn-primary'));
    expect(submitButton.nativeElement.type).toBe('button');
  });

  it('should have borderless footer', () => {
    const footer = fixture.debugElement.query(By.css('.modal-footer'));
    expect(footer.nativeElement.classList).toContain('border-0');
  });

  it('should have no horizontal padding', () => {
    const footer = fixture.debugElement.query(By.css('.modal-footer'));
    expect(footer.nativeElement.classList).toContain('px-0');
  });
}); 
import { Component, OnInit } from '@angular/core';
import { Task } from '../../models/task.model';
import { TaskService } from '../../services/task.service';
import { UserService } from '../../services/user.service';

type usersByIdType = {
  [key: number]: string
};

@Component({
  selector: 'app-task',
  templateUrl: './task.component.html',
  styleUrls: ['./task.component.scss']
})
export class TaskComponent implements OnInit {
  tasks: Task[] = [];
  users: usersByIdType = {};
  formTask: Task;
  usersTasks: {[key: number]: number};

  constructor(private taskService: TaskService, private userService: UserService) {
    this.formTask = this.initializeNewTask();
    this.usersTasks = {};
  }

  ngOnInit(): void {
    this.loadTasks();
    this.loadUsers();
    this.loadUserTasks();
  }

  loadTasks(): void {
    this.taskService.getTasks().subscribe((tasks) => (this.tasks = tasks));
  }

  loadUsers(): void {
    this.userService.getUsers().subscribe((users) => {
      let usersById: usersByIdType = {};
      users.forEach((user) => {
        usersById[user.id] = user.name
      });
      this.users = usersById;
    });
  }

  createTask(): void {
    // todo: show alert on error
    this.taskService.createTask(this.formTask).subscribe(() => {
      this.loadTasks();
      this.formTask = this.initializeNewTask();
    });
  }

  onSelected(event: any): void {
    this.formTask.userId = parseInt(event.target.value);
  }

  initializeNewTask() {
    return { id: 0, userId: 0, subject: '', isCompleted: false, targetDate: '' };
  }

  loadUserTasks(): void {
    this.userService.getUsersTasks().subscribe((usersTasks) => (this.usersTasks = usersTasks));
  }

  returnInt(value: any): number {
    return parseInt(value);
  }
}

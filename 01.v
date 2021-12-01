import os

fn count_increases(lines []string, window_size int) int {
	mut window := []int{}
	mut window_sum := 0
	mut increase_count := 0
	for line in lines {
		old_window_sum := window_sum
		window << line.int()
		window_sum += line.int()
		if window.len > window_size {
			window_sum -= window.first()
			window = window[1..]
			if old_window_sum < window_sum {
				increase_count++
			}
		}
	}
	return increase_count
}

fn main() {
	lines := os.read_lines('01_input.txt') ?
	println(count_increases(lines, 1))
	println(count_increases(lines, 3))
}

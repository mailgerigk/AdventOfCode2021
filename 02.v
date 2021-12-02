import os

fn part1(lines []string) {
	mut h := 0
	mut d := 0
	for line in lines {
		parts := line.split(' ')
		if parts.len != 2 {
			continue
		}
		if parts[0] == 'forward' {
			h += parts[1].int()
		} else if parts[0] == 'up' {
			d -= parts[1].int()
		} else {
			d += parts[1].int()
		}
	}
	println(h * d)
}

fn part2(lines []string) {
	mut h := 0
	mut d := 0
	mut a := 0
	for line in lines {
		parts := line.split(' ')
		if parts.len != 2 {
			continue
		}
		if parts[0] == 'forward' {
			h += parts[1].int()
			d += a * parts[1].int()
		} else if parts[0] == 'up' {
			a -= parts[1].int()
		} else {
			a += parts[1].int()
		}
	}
	println(h * d)
}

fn main() {
	lines := os.read_lines('02_input.txt') ?
	part1(lines)
	part2(lines)
}

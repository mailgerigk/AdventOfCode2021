import os

fn part1(lines []string) {
	line_length := lines.first().len
	mut gamma := []int{len: line_length}
	for l := 0; l < line_length; l++ {
		mut acc := 0
		for i := 0; i < lines.len; i++ {
			if lines[i][l] == 49 {
				acc++
			}
		}
		if acc > lines.len / 2 {
			gamma[l] = 1
		}
		if acc == lines.len / 2 {
			gamma[l] = 2
		}
	}
	mut oxy_lines := lines.clone()
	mut co2_lines := lines.clone()
	mut next_oxy_lines := []string{}
	mut next_co2_lines := []string{}
	mut oxy_rating := 0
	mut co2_rating := 0
	for l := 0; l < line_length; l++ {
		if gamma[l] == 1 {
			for line in oxy_lines {
				if line[l] == 49 {
					next_oxy_lines << line
				}
			}
			for line in co2_lines {
				if line[l] == 48 {
					next_co2_lines << line
				}
			}
		}
		if gamma[l] == 0 {
			for line in oxy_lines {
				if line[l] == 48 {
					next_oxy_lines << line
				}
			}
			for line in co2_lines {
				if line[l] == 49 {
					next_co2_lines << line
				}
			}
		}
		if gamma[l] == 2 {
			for line in oxy_lines {
				if line[l] == 49 {
					next_oxy_lines << line
				}
			}
			for line in co2_lines {
				if line[l] == 48 {
					next_co2_lines << line
				}
			}
		}
		oxy_lines = next_oxy_lines.clone()
		next_oxy_lines = []string{}
		println('oxy $oxy_lines')
		co2_lines = next_co2_lines.clone()
		next_co2_lines = []string{}

		if oxy_lines.len == 1 {
			for c in oxy_lines.first() {
				oxy_rating <<= 1
				oxy_rating |= if c == 49 { 1 } else { 0 }
			}
		}
		if co2_lines.len == 1 {
			println('co2 $co2_lines')
			for c in co2_lines.first() {
				co2_rating <<= 1
				co2_rating |= if c == 49 { 1 } else { 0 }
			}
		}
	}
	println(oxy_rating * co2_rating)
}

fn part2(lines []string) {
}

fn main() {
	lines := os.read_lines('03_example.txt') ?
	part1(lines)
	part2(lines)
}

using System;
using System.Collections.Generic;
using System.Linq;

public enum Button {
	Start,
	Cancel,
	Block,
	Dodge,
	Push,
	Shove,
	Pause = Start,
}

public static class Buttons {
	public static IList<string> GetNames() {
		var names = Enum.GetNames(typeof(Button)).ToList();
		names.Remove(nameof(Button.Pause));
		return names;
	}

	public static IList<int> GetValues() {
		var array = Enum.GetValues(typeof(Button));
		var copy = new int[array.Length];
		return copy;
	}
}
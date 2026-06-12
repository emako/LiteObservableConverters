[![NuGet](https://img.shields.io/nuget/v/LiteObservableConverters.svg)](https://nuget.org/packages/LiteObservableConverters) [![Actions](https://github.com/emako/LiteObservableConverters/actions/workflows/library.nuget.yml/badge.svg)](https://github.com/emako/LiteObservableConverters/actions/workflows/library.nuget.yml)

# LiteObservableConverters

Lite version of [ComputedConverters.WPF](https://github.com/emako/ComputedConverters.NET) with fewer features but better performance and a smaller surface area.

## Features

- Common WPF `IValueConverter` / `IMultiValueConverter` implementations for MVVM bindings.
- `ValueConverterGroup` for chaining converters in XAML.
- Markup extensions for XAML constants (visibility, primitives, etc.).
- XAML namespace: `xmlns:loc="http://schemas.github.com/liteobservableconverters/2026/xaml"`.

## Usage

> Implementation in progress. See `src/WpfApp` for upcoming demos.

```xaml
xmlns:loc="http://schemas.github.com/liteobservableconverters/2026/xaml"
```

## Project layout

| Path | Purpose |
|------|---------|
| `src/LiteObservableConverters/Converters` | Value / multi-value converters |
| `src/LiteObservableConverters/Markups` | XAML markup extensions |
| `src/LiteObservableConverters/Common` | Shared helpers |
| `src/LiteObservableConverters/Extensions` | Extension methods |
| `src/WpfApp` | WPF demo application |

## License

[MIT](LICENSE)

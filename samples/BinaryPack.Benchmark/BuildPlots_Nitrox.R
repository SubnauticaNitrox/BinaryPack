BenchmarkDotNetVersion <- "BenchmarkDotNet v0.13.1 "
dir.create(Sys.getenv("R_LIBS_USER"), recursive = TRUE, showWarnings = FALSE)
list.of.packages <- c("ggplot2", "dplyr", "gdata", "grid", "gridExtra", "Rcpp", "R.devices")
new.packages <- list.of.packages[!(list.of.packages %in% installed.packages()[,"Package"])]
if(length(new.packages)) install.packages(new.packages, lib = Sys.getenv("R_LIBS_USER"), repos = "https://cran.rstudio.com/")
library(ggplot2)
library(dplyr)
library(gdata)
library(grid)
library(gridExtra)
library(R.devices)

isEmpty <- function(val){
   is.null(val) | val == ""
}

createPrefix <- function(params){
   separator <- "-"
   values <- params[!isEmpty(params)]
   paste(replace(values, TRUE, paste0(separator, values)), collapse = "")
}

ends_with <- function(vars, match, ignore.case = TRUE) {
  if (ignore.case)
    match <- tolower(match)
  n <- nchar(match)

  if (ignore.case)
    vars <- tolower(vars)
  length <- nchar(vars)

  substr(vars, pmax(1, length - n + 1), length) == match
}

BenchmarkDotNetVersionGrob <- textGrob(BenchmarkDotNetVersion, gp = gpar(fontface=3, fontsize=10), hjust=1, x=1)
nicePlot <- function(p) grid.arrange(p, bottom = BenchmarkDotNetVersionGrob)
ggsaveNice <- function(fileName, p, ...) {
  cat(paste0("*** Plot: ", fileName, " ***\n"))
  # See https://stackoverflow.com/a/51655831/184842
  suppressGraphics(ggsave(fileName, plot = nicePlot(p), ...))
  cat("------------------------------\n")
}

args <- commandArgs(trailingOnly = TRUE)
files <- if (length(args) > 0) args else list.files()[list.files() %>% ends_with("-report.csv")]
for (file in files) {
  title <- "SubnauticaNitrox.BinaryPack"
  result <- read.csv(file, sep = ";", encoding="UTF-8")

  if (nrow(result[is.na(result$Job),]) > 0)
    result[is.na(result$Job),]$Job <- ""
  if (nrow(result[is.na(result$Params),]) > 0) {
    result[is.na(result$Params),]$Params <- ""
  } else {
    result$Job <- trim(paste(result$Job, result$Params))
  }
  result$Job <- factor(result$Job, levels = unique(result$Job))

  timeunit <- sub("([0-9.,\n]+ )", "", result$Mean)

  result_stats <- result %>%
    mutate(numericalMean = as.numeric(gsub(",", "", gsub("([0-9.,]+).*$", "\\1", Mean)))) %>%
    mutate(numericalStdDev = as.numeric(gsub(",", "", gsub("([0-9.,]+).*$", "\\1", StdDev)))) %>%
    group_by(paste(Method, Job, Categories))

  binaryPackStats <- result_stats %>%
    filter(Method == "BinaryPack" | Method == "MessagePack")

  benchmarkBarplot <- ggplot(result_stats, aes(x=Method, y=numericalMean, fill=paste(Job, Categories))) +
    guides(fill=guide_legend(title="Job")) +
    xlab("Target") +
    ylab(paste("Time,", timeunit)) +
    ggtitle(title) +
    geom_bar(position=position_dodge(), stat="identity", colour="black") +
    geom_errorbar(aes(ymin=numericalMean-numericalStdDev, ymax=numericalMean+numericalStdDev), width=.4, position=position_dodge(.9)) +
    scale_fill_manual(values = c("red", "darkred", "blue", "darkblue")) +
    theme(axis.text.x = element_text(angle = 30, hjust = 1))

  benchmarkBarplotBinaryBack <- ggplot(binaryPackStats, aes(x=Method, y=numericalMean, fill=paste(Job, Categories))) +
    guides(fill=guide_legend(title="Job")) +
    xlab("Target") +
    ylab(paste("Time,", timeunit)) +
    ggtitle(title) +
    geom_bar(position=position_dodge(), stat="identity", colour="black") +
    geom_errorbar(aes(ymin=numericalMean-numericalStdDev, ymax=numericalMean+numericalStdDev), width=.4, position=position_dodge(.9)) +
    scale_fill_manual(values = c("red", "darkred", "blue", "darkblue")) +
    theme(axis.text.x = element_text(angle = 30, hjust = 1)) +
    coord_flip()


  ggsaveNice(gsub("BinaryPack.Benchmark.Implementations.Benchmark_JsonResponseModel_-report.csv", "BinaryPack.Benchmark-barplot.png", file), benchmarkBarplot)
  ggsaveNice(gsub("BinaryPack.Benchmark.Implementations.Benchmark_JsonResponseModel_-report.csv", "BinaryPack.Benchmark-binarypackplot.png", file), benchmarkBarplotBinaryBack)
}

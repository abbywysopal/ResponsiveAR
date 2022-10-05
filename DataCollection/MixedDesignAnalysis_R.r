rm(list = ls())

#library(FSA) # <?>
#library(DescTools)
#library(tidyr)
#library(tidyverse)

library(plyr)
library(dplyr)
library(ggplot2)
library(ggpubr)
library(rstatix)

# Unix style: 
# my_data <- read.csv("/Users/holl/Dropbox/Documents/2022-AbbyMS/ResponsiveARExperimentData.csv", header = TRUE, fileEncoding = 'UTF-8-BOM')

# Windows style: 
my_data <- read.csv("ExperimentData2.csv", header= TRUE, fileEncoding = 'UTF-8-BOM')


# averaging over all the tasks per application (since there is an unequal number of tasks it cannot easily be a separate within-subject variable)
f <- ddply(select(my_data, -Task),.(userID, Mode, Application),colwise(mean)) 

# Perform ANOVA
res.aov <- anova_test( data = f, dv = Time, wid = userID, within = Application, between = Mode)


# Some visualizations: bar plot with CI whiskers 
ggplot(my_data, aes(x=Application, y=Time)) + geom_bar(stat = "summary", fun = "mean", fill="lightsteelblue") + geom_errorbar(stat="summary", fun.data="mean_se", fun.args = list(mult = 1.96))
ggplot(my_data, aes(x=Mode, y=Time)) + geom_bar(stat = "summary", fun = "mean", fill="lightsteelblue") + geom_errorbar(stat="summary", fun.data="mean_se", fun.args = list(mult = 1.96))
# all data in box plots: 
ggboxplot(my_data, x = "Mode", y = "Time", color = "Application", palette = "jco")
# aggregated over task: 
ggboxplot(f, x = "Mode", y = "Time", color = "Application", palette = "jco")


res.aov <- anova_test( data = f, dv = Time, wid = userID, between = Mode, within = Application)
get_anova_table(res.aov)

pwc <- f %>% group_by(Application) %>% pairwise_t_test(Time ~ Mode, p.adjust.method = "bonferroni")
pwc